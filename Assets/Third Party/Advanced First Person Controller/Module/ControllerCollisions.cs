using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Game
{
	public class ControllerCollisions : FirstPersonController.Module
	{
        #region Collection
        public List<Collision> List { get; protected set; }

        public int Count => List.Count;

        public Collision this[int index] => List[index];

        protected virtual void Set(UnityEngine.Collision collision) => Set(new Collision(collision));
        protected virtual void Set(Collision collision)
        {
            var index = List.FindIndex(IsMatch);

            if (index < 0)
                Add(collision);
            else
                List[index] = collision;

            bool IsMatch(Collision element) => element.collider == collision.collider;
        }

        protected virtual void Add(Collision collision)
        {
            List.Add(collision);
        }

        protected virtual void Remove(UnityEngine.Collision collision) => Remove(new Collision(collision));
        protected virtual void Remove(Collision collision)
        {
            List.RemoveAll(IsMatch);

            bool IsMatch(Collision element) => element.collider == collision.collider;
        }
        #endregion

        [Serializable]
        public class Collision
        {
            public Vector3 relativeVelocity;
            public Rigidbody rigidbody;
            public Collider collider;
            public Transform transform;
            public GameObject gameObject;
            public int contactCount;
            public ContactPoint[] contacts;
            public Vector3 impulse;

            public Collision(UnityEngine.Collision collision)
            {
                relativeVelocity = collision.relativeVelocity;
                rigidbody = collision.rigidbody;
                collider = collision.collider;
                transform = collision.transform;
                gameObject = collision.gameObject;
                contactCount = collision.contactCount;
                contacts = ContactPoint.ConvertAll(collision.contacts);
                impulse = collision.impulse;
            }
        }

        [Serializable]
        public class ContactPoint
        {
            public Vector3 point;
            public Vector3 normal;
            public Collider thisCollider;
            public Collider otherCollider;
            public float separation;

            public ContactPoint(UnityEngine.ContactPoint contact)
            {
                point = contact.point;
                normal = contact.normal;
                thisCollider = contact.thisCollider;
                otherCollider = contact.otherCollider;
                separation = contact.separation;
            }

            public static ContactPoint[] ConvertAll(IList<UnityEngine.ContactPoint> list)
            {
                var result = new ContactPoint[list.Count];

                for (int i = 0; i < list.Count; i++)
                    result[i] = new ContactPoint(list[i]);

                return result;
            }
        }

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            List = new List<Collision>();
        }

        public override void Init()
        {
            base.Init();

            Controller.PhysicsCallbacks.CollisionEnterEvent += Set;
            Controller.PhysicsCallbacks.CollisionStayEvent += Set;
            Controller.PhysicsCallbacks.CollisionExitEvent += Remove;

            Controller.OnProcess += Process;
        }

        void Process()
        {
            
        }

        void OnDrawGizmosSelected()
        {
            if(Application.isPlaying)
            {
                Gizmos.color = Color.blue;

                for (int x = 0; x < List.Count; x++)
                    for (int y = 0; y < List[x].contactCount; y++)
                        DrawContact(List[x].contacts[y]);
            }

            void DrawContact(ContactPoint contact)
            {
                Gizmos.DrawRay(contact.point, contact.normal * 0.2f);
            }
        }
    }
}