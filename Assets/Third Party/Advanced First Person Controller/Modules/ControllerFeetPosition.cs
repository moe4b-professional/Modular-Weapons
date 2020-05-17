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
    public class ControllerFeetPosition : FirstPersonController.Module
    {
        public Vector3 Value { get; protected set; }

        public Transform Point { get; protected set; }

        public ControllerState State => Controller.State;

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Point = transform.Find("Point");

            if (Point == null)
            {
                Point = new GameObject("Point").transform;
                Point.SetParent(transform);
            }
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Value = Calculate(Controller.State);

            Point.position = Value;
        }

        public virtual Vector3 Calculate(ControllerState.Data data) => Calculate(data.Height, data.Radius);
        public virtual Vector3 Calculate(ControllerState state) => Calculate(state.Height, state.Radius);
        public virtual Vector3 Calculate(float height, float radius)
        {
            var value = Controller.collider.transform.position;

            value -= Controller.transform.up * (height / 2f);

            return value;
        }
    }
}