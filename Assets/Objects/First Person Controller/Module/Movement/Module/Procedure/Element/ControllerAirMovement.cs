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
        protected bool control = true;
        public bool Control { get { return control; } }

        public Vector3 Target { get; protected set; }

        protected override void Process()
        {
            base.Process();

            if (Active)
            {
                Input.Calcaulate();

                Jump.Operate();

                Sprint.Operate();

                State.Operate();
            }
        }

        protected override void FixedProcess()
        {
            base.FixedProcess();

            if (Active)
            {
                Speed.Calculate();

                Acceleration.Calculate();

                Gravity.Apply();

                if(control)
                {
                    CalculateTarget();

                    Velocity.Absolute = Vector3.MoveTowards(Velocity.Absolute, Target, Acceleration.Value * Time.deltaTime);
                }

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