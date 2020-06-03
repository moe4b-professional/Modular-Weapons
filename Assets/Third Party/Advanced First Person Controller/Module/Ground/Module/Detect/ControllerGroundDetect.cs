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
    public class ControllerGroundDetect : ControllerGround.Module
    {
        [SerializeField]
        protected LayerMask mask = Physics.DefaultRaycastLayers;
        public LayerMask Mask { get { return mask; } }

        [SerializeField]
        protected float range = 0.2f;
        public float Range { get { return range; } }

        public float Offset => 0.1f + Step.MaxHeight;

        protected virtual Vector3 CalculateOrigin(int iteration)
        {
            var origin = Controller.transform.position;

            origin += Direction.Down * Controller.Height / 2f;
            origin += Direction.Up * Offset;
            origin += Direction.Up * Controller.Radius / iteration;

            if (Controller.Movement.Speed.Max > 0f)
            {
                var direction = Vector3.ClampMagnitude(Controller.Velocity.Planar / Controller.Movement.Speed.Max, 1f);
                origin += direction * (Controller.Radius / 2f / iteration);
            }

            return origin;
        }

        public float MaxDistance => range + Offset;

        public ControllerGroundData Data { get; protected set; }

        public ControllerGroundSlopeDetect Slope { get; protected set; }

        public ControllerGroundStepDetect Step { get; protected set; }

        public const int MaxIterations = 3;

        public class Module : ControllerGround.Module
        {
            public ControllerGroundDetect Detect => Ground.Detect;
        }

        public ControllerDirection Direction => Controller.Direction;

        public override void Configure()
        {
            base.Configure();

            Slope = Dependancy.Get<ControllerGroundSlopeDetect>(Controller.gameObject);

            Step = Dependancy.Get<ControllerGroundStepDetect>(Controller.gameObject);
        }

        public delegate void ProcessDelegate(ControllerGroundData hit);
        public event ProcessDelegate OnProcess;
        public virtual void Process()
        {
            for (int i = 1; i <= MaxIterations; i++)
            {
                if (Cast(i, out var hit))
                {
                    Data = Calculate(hit);

                    if (IsValid(Data))
                        break;
                    else
                        Data = null;
                }
                else
                    Data = null;
            }

            OnProcess?.Invoke(Data);
        }

        protected virtual bool Cast(int iteration, out RaycastHit hit)
        {
            var origin = CalculateOrigin(iteration);

            if (Physics.SphereCast(origin, Controller.Radius / iteration, Direction.Down, out hit, MaxDistance, mask, QueryTriggerInteraction.Ignore))
                return true;
            else
                return false;
        }

        protected virtual ControllerGroundData Calculate(RaycastHit hit)
        {
            if (hit.collider == null) return null;

            var angle = Slope.CalculateAngle(hit.normal);
            var stepHeight = Step.CalculateHeight(hit.point);

            return new ControllerGroundData(hit, angle, stepHeight);
        }

        protected virtual bool IsValid(ControllerGroundData data)
        {
            if (Slope.IsValid(data))
                return true;

            if (Step.IsValid(data))
                return true;

            return false;
        }

        #region Gizmos
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                var origin = CalculateOrigin(1);

                var end = origin + Direction.Down * MaxDistance;

                Handles.color = Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(origin, Controller.Radius);

                Handles.color = Gizmos.color = Color.green;
                DrawWireCapsule(origin, end, Controller.Radius);
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