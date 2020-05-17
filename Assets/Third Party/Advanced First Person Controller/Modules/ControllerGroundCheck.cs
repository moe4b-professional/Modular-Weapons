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

using Collision = Game.ControllerCollisions.Collision;
using ContactPoint = Game.ControllerCollisions.ContactPoint;

namespace Game
{
	public class ControllerGroundCheck : FirstPersonController.Module
	{
        [SerializeField]
        protected float maxSlope = 60f;
        public float MaxSlope { get { return maxSlope; } }

        public HitData Hit { get; protected set; }

        public bool IsGrounded => Hit != null;

        public ControllerCollisions Collisions => Controller.Collisions;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Hit = Check(Collisions.List);
        }

        protected virtual HitData Check(IList<Collision> list)
        {
            if (Collisions.Count == 0) return null;

            for (int i = 0; i < list.Count; i++)
            {
                var hit = Check(list[i]);

                if (hit == null) continue;

                return hit;
            }

            return null;
        }
        protected virtual HitData Check(Collision collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                var hit = Check(collision, collision.contacts[i]);

                if (hit == null) continue;

                return hit;
            }

            return null;
        }
        protected virtual HitData Check(Collision collision, ContactPoint contact)
        {
            var angle = CalculateAngle(contact.normal);

            if (angle > maxSlope) return null;

            return new HitData(collision, contact, angle);
        }

        protected float CalculateAngle(Vector3 normal)
        {
            return Vector3.Angle(Controller.transform.up, normal);
        }

        [Serializable]
        public class HitData
        {
            public Vector3 Point { get; protected set; }

            public Vector3 Normal { get; protected set; }

            public float Angle { get; protected set; }

            public HitData(Collision collision, ContactPoint contact, float angle)
            {
                Point = contact.point;

                Normal = contact.normal;

                this.Angle = angle;
            }
        }
    }
}