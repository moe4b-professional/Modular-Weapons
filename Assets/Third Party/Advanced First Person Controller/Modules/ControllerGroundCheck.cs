﻿using System;
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
    public class ControllerGroundCheck : FirstPersonController.Module
    {
        [SerializeField]
        protected LayerMask mask = Physics.DefaultRaycastLayers;
        public LayerMask Mask { get { return mask; } }

        [SerializeField]
        protected float range = 0.2f;
        public float Range { get { return range; } }

        [SerializeField]
        protected float offset = 0.1f;
        public float Offset { get { return offset; } }

        public float MaxDistance => range + offset + (Controller.State.Height / 2f);

        public float Radius => Controller.State.Radius;

        [SerializeField]
        [Range(0f, 90f)]
        protected float maxSlope = 50f;
        public float MaxSlope { get { return maxSlope; } }

        [SerializeField]
        protected float maxStepHeight = 0.3f;
        public float StepHeight { get { return maxStepHeight; } }

        public Vector3 Origin { get; protected set; }

        public Vector3 Direction => -Controller.transform.up;

        public HitData Hit { get; protected set; }

        public bool IsGrounded => Hit != null;

        public ControllerAirTravel AirTravel => Controller.AirTravel;

        public override void Init()
        {
            base.Init();

            Detect();

            if (IsGrounded == false) LeftGround();
        }

        public virtual void Do()
        {
            var oldHit = Hit;

            Detect();

            ProcessChange(oldHit, Hit);
        }

        #region Detect
        protected virtual void Detect()
        {
            for (int i = 1; i <= 3; i++)
            {
                CalculateOrigin(i);

                if (Physics.SphereCast(Origin, Radius / i, Direction, out var hit, MaxDistance, mask, QueryTriggerInteraction.Ignore))
                {
                    Hit = Check(hit);
                }
                else
                {
                    Hit = null;
                    break;
                }

                if (Hit != null) break;
            }
        }

        protected virtual void CalculateOrigin(int index)
        {
            Origin = Controller.transform.position + (-Direction * (offset + (Radius / index)));

            Origin += Vector3.ClampMagnitude(Controller.rigidbody.velocity / Controller.Movement.Speed, 1f) * (Controller.State.Radius / 2f / index);
        }

        protected virtual HitData Check(RaycastHit hit)
        {
            if (hit.collider == null) return null;

            Debug.DrawRay(hit.point, hit.normal * 0.5f, Color.blue);

            var angle = Vector3.Angle(Controller.transform.up, hit.normal);
            var stepHeight = Controller.transform.InverseTransformPoint(hit.point).y + (Controller.State.Height / 2f);

            var result = new HitData(hit, angle, stepHeight);

            if (angle > maxSlope)
            {
                if (CheckSlope(result))
                    return result;
                return result;
            }

            return result;
        }

        protected virtual bool CheckSlope(HitData hit)
        {
            return true;
        }
        #endregion

        #region Change
        protected virtual void ProcessChange(HitData oldState, HitData newState)
        {
            if(oldState == null && newState != null) //Landed On Ground
                Landed();
            else if(oldState != null && newState == null) //Left Ground
                LeftGround();
        }

        public delegate void LeftGroundDelegate();
        public event LeftGroundDelegate OnLeftGround;
        protected virtual void LeftGround()
        {
            Debug.Log("Left Ground");

            AirTravel.Begin();

            OnLeftGround?.Invoke();
        }

        public delegate void LandingDelegate(ControllerAirTravel.Data travel);
        public event LandingDelegate OnLanding;
        protected virtual void Landed()
        {
            Debug.Log("Landed On Ground");

            var travel = AirTravel.End();

            OnLanding?.Invoke(travel);
        }
        #endregion

        [Serializable]
        public class HitData
        {
            public Vector3 Point { get; protected set; }

            public Vector3 Normal { get; protected set; }

            public float Angle { get; protected set; }

            public float StepHeight { get; protected set; }

            public HitData(Vector3 point, Vector3 normal, float angle, float stepHeight)
            {
                this.Point = point;
                this.Normal = normal;
                this.Angle = angle;
                this.StepHeight = stepHeight;
            }
            public HitData(Collision collision, ContactPoint contact, float angle, float stepHeight) : this(contact.point, contact.normal, angle, stepHeight) { }
            public HitData(RaycastHit hit, float angle, float stepHeight) : this(hit.point, hit.normal, angle, stepHeight) { }
        }

        #region Gizmos
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                var end = Origin + Direction * MaxDistance;

                Handles.color = Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(Origin, Radius);

                Handles.color = Gizmos.color = Color.green;
                DrawWireCapsule(Origin, end, Radius);
            }
        }

        public static void DrawWireCapsule(Vector3 start, Vector3 end, float radius)
        {
            var forward = end - start;
            var rotation = Quaternion.LookRotation(forward);
            var pointOffset = radius / 2f;
            var length = forward.magnitude;
            var center2 = new Vector3(0f, 0, length);

            Matrix4x4 angleMatrix = Matrix4x4.TRS(start, rotation, Handles.matrix.lossyScale);

            using (new Handles.DrawingScope(angleMatrix))
            {
                Handles.DrawWireDisc(Vector3.zero, Vector3.forward, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.up, Vector3.left * pointOffset, -180f, radius);
                Handles.DrawWireArc(Vector3.zero, Vector3.left, Vector3.down * pointOffset, -180f, radius);
                Handles.DrawWireDisc(center2, Vector3.forward, radius);
                Handles.DrawWireArc(center2, Vector3.up, Vector3.right * pointOffset, -180f, radius);
                Handles.DrawWireArc(center2, Vector3.left, Vector3.up * pointOffset, -180f, radius);

                DrawLine(radius, 0f, length);
                DrawLine(-radius, 0f, length);
                DrawLine(0f, radius, length);
                DrawLine(0f, -radius, length);
            }
        }

        public static void DrawLine(float arg1, float arg2, float forward)
        {
            Handles.DrawLine(new Vector3(arg1, arg2, 0f), new Vector3(arg1, arg2, forward));
        }
#endif
        #endregion
    }
}