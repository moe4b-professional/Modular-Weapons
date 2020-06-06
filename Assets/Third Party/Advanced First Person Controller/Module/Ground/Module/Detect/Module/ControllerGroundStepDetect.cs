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
	public class ControllerGroundStepDetect : ControllerGroundDetect.Module
	{
        [SerializeField]
        protected float maxHeight = 0.3f;
        public float MaxHeight { get { return maxHeight; } }

        public LayerMask Mask => Controller.GenericData.LayerMask;

        public float Offset => 0.1f;

        public float Range => maxHeight + (Offset * 2f);

        public ControllerDirection Direction => Controller.Direction;

        public float CalculateHeight(Vector3 point) => Controller.transform.InverseTransformPoint(point).y + (Controller.Height / 2f);

        public virtual bool IsValid(ControllerGroundData data)
        {
            if (data.StepHeight > maxHeight) return false;

            var origin = CalculateOrigin(data.Point);

            if (Physics.Raycast(origin, Direction.Down, out var hit, Range, Detect.Mask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawRay(origin, Direction.Down * Range, Color.green, 10f);

                var angle = Vector3.Angle(Direction.Up, hit.normal);

                return Detect.Slope.IsValid(angle);
            }
            else
            {
                Debug.DrawRay(origin, Direction.Down * Range, Color.red, 10f);

                return false;
            }
        }

        public Vector3 CalculateOrigin(Vector3 point)
        {
            var result = point;

            result += Direction.Up * Offset;

            result += Controller.Velocity.Planar.normalized * Controller.Radius / 4f;

            return result;
        }
    }
}