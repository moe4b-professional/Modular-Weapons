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
	public class ControllerMovement : FirstPersonController.Module
    {
        [SerializeField]
        protected float acceleration = 15f;
        public float Acceleration { get { return acceleration; } }

        public Vector3 Target { get; protected set; }

        public ControllerMovementInput Input { get; protected set; }
        public ControllerMovementSpeed Speed { get; protected set; }
        public ControllerDirection Direction { get; protected set; }

        public ControllerGround Ground => Controller.Ground;
        public ControllerGravity Gravity => Controller.Gravity;
        public ControllerVelocity Velocity => Controller.Velocity;
        public ControllerState State => Controller.State;

        public class Module : FirstPersonController.Module
        {
            public ControllerMovement Movement => Controller.Movement;
        }

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Input = Dependancy.Get<ControllerMovementInput>(Controller.gameObject);
            Speed = Dependancy.Get<ControllerMovementSpeed>(Controller.gameObject);
            Direction = Dependancy.Get<ControllerDirection>(Controller.gameObject);
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
            Controller.OnFixedProcess += FixedProcess;
        }

        void Process()
        {
            Input.Calcaulate();
        }

        public float Multiplier { get; protected set; }

        void FixedProcess()
        {
            Gravity.Apply();
            Ground.Check();

            if (Ground.IsGrounded)
                Multiplier = State.Multiplier;

            Speed.Process(Multiplier);

            Target = CalculateTarget();

            Velocity.Absolute = Vector3.MoveTowards(Velocity.Absolute, Target, acceleration * State.Multiplier * Time.deltaTime);

            Debug.DrawRay(Controller.transform.position, Target, Color.yellow);
            Debug.DrawRay(Controller.transform.position, Velocity.Absolute, Color.red);
        }

        protected virtual Vector3 CalculateTarget()
        {
            var result = Vector3.ClampMagnitude(Input.Absolute * Speed.Max, Speed.Max);

            if (Ground.IsGrounded)
                result = Vector3.ProjectOnPlane(result, Ground.Data.Normal);

            result += Controller.Velocity.Calculate(Gravity.Direction);

            return result;
        }
    }
}