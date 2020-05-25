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
	public class ControllerStateRoofCheck : ControllerState.Module
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

        public float Radius => Controller.State.Radius * 0.8f;
        public float Height => Controller.State.Height;

        public ControllerDirection Direction => Controller.Direction;

        protected virtual Vector3 CalculateOrigin()
        {
            var origin = Controller.transform.position;

            origin += Direction.Up * Height / 2f;
            origin += Direction.Down * offset;
            origin += Direction.Down * Radius;

            return origin;
        }

        public float MaxDistance => range + offset;

        public virtual bool Active
        {
            get
            {
                if (Transition.Target.Weight == 1f) return false;

                var delta = Transition.Target.Height - State.Height;

                if (delta < 0.001f) return false;

                return true;
            }
        }

        public ControllerStateTransition Transition => State.Transition;

        public IList<BaseControllerStateElement> Elements => State.Elements;

        public override void Init()
        {
            base.Init();

            Controller.OnFixedProcess += Process;
        }

        void Process()
        {
            if(Active)
            {
                if (Detect())
                {
                    var target = FindElement(State.Height);

                    if (target == null)
                    {

                    }
                    else
                    {
                        Transition.Set(target);
                    }
                }
            }
        }

        protected virtual BaseControllerStateElement FindElement(float height)
        {
            for (int i = 0; i < Elements.Count; i++)
                if (Elements[i].Height < height)
                    return Elements[i];

            return null;
        }

        protected virtual bool Detect()
        {
            var origin = CalculateOrigin();

            if(Physics.SphereCast(origin, Radius, Direction.Up, out var hit, MaxDistance, mask, QueryTriggerInteraction.Ignore))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Gizmos
#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                var origin = CalculateOrigin();

                var end = origin + Direction.Up * MaxDistance;

                Handles.color = Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(origin, Radius);

                Handles.color = Gizmos.color = Color.green;
                DrawWireCapsule(origin, end, Radius);
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