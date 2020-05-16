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
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class Projectile : MonoBehaviour
	{
        public Rigidbody rigidbody { get; protected set; }

        public Collider collider { get; protected set; }

        public Bounds Bounds => collider.bounds;

        public float Radius => (Bounds.size.x + Bounds.size.y + Bounds.size.z) / 3f;

        public bool Armed { get; protected set; }
        public virtual void Arm()
        {
            Armed = true;
        }

        public virtual void DisArm()
        {
            Armed = false;
        }

        public virtual void Configure()
        {
            rigidbody = GetComponent<Rigidbody>();

            collider = GetComponent<Collider>();
        }

        public virtual void AddVelocity(Vector3 direction, float value)
        {
            rigidbody.AddForce(direction * value, ForceMode.VelocityChange);
        }

        public virtual void IgnoreCollisions(GameObject target)
        {
            var targets = Dependancy.GetAll<Collider>(target);

            for (int i = 0; i < targets.Count; i++)
                Physics.IgnoreCollision(collider, targets[i], true);
        }

        void OnCollisionEnter(Collision collision)
        {
            if(Armed)
            {
                var data = new HitData(collision.collider, collision.contacts[0], rigidbody.velocity.normalized);

                ProcessHit(data);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (Armed)
            {
                var contact = new HitContact(transform.forward * (Radius / 2f), -transform.forward);

                var data = new HitData(collider, contact, rigidbody.velocity.normalized);

                ProcessHit(data);
            }
        }

        public delegate void HitDelegate(Projectile projectile, HitData data);
        public event HitDelegate OnHit;
        protected virtual void ProcessHit(HitData data)
        {
            OnHit?.Invoke(this, data);
        }

        public event Action OnDestroy;
        public virtual void Destroy()
        {
            if (Armed) DisArm();

            OnDestroy?.Invoke();

            Destroy(gameObject);
        }
    }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}