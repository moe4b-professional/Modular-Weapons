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
	public class ControllerAirMovement : ControllerMovementProcedure.Element
    {
        [SerializeField]
        protected float acceleration = 15f;
        public float Acceleration { get { return acceleration; } }

        public Vector3 Target { get; protected set; }

        protected override void Process()
        {
            base.Process();

            if (Active)
            {
                Input.Calcaulate();
            }
        }

        protected override void FixedProcess()
        {
            base.FixedProcess();

            if (Active)
            {
                Gravity.Apply();

                CalculateTarget();

                Velocity.Absolute = Vector3.MoveTowards(Velocity.Absolute, Target, acceleration * Time.deltaTime);

                Debug.DrawRay(Controller.transform.position, Target, Color.yellow);
                Debug.DrawRay(Controller.transform.position, Velocity.Absolute, Color.red);
            }
        }

        protected virtual void CalculateTarget()
        {
            Target = Vector3.ClampMagnitude(Input.Absolute * Speed.Max, Speed.Max);

            Target += Velocity.Calculate(Gravity.Direction);
        }
    }
}