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
	public class ControllerStateHeightAdjustment : ControllerState.Module
	{
        public Transform Point { get; protected set; }

        public override void Configure(ControllerState reference)
        {
            base.Configure(reference);

            Point = transform.Find("Point");

            if(Point == null)
            {
                Point = new GameObject("Point").transform;
                Point.SetParent(transform);
            }
        }

        public virtual void Process(ControllerState.Data target)
        {
            var delta = CalculateHeight(State) - CalculateHeight(target);

            Controller.transform.position += Controller.transform.up * delta;
        }

        protected virtual float CalculateHeight(ControllerState state) => CalculateHeight(state.Height, state.Radius, state.Angle.Value);
        protected virtual float CalculateHeight(ControllerState.Data data) => CalculateHeight(data.Height, data.Radius, data.Angle);
        protected virtual float CalculateHeight(float height, float radius, float angle)
        {
            Apply(height, radius, angle);

            return Point.position.y;
        }

        protected virtual void Apply(float height, float radius, float angle)
        {
            Point.position = Controller.collider.transform.position;
            Point.rotation = Controller.transform.rotation * Quaternion.Euler(angle, 0f, 0f);

            Point.position -= Point.up * (height / 2f);

            var anchor = Point.position + (Point.up * radius);

            Point.RotateAround(anchor, Controller.transform.right, -angle);
        }
	}
}