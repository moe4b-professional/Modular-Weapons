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

        public Vector3 Input { get; protected set; }

        public Vector3 Target { get; protected set; }

        public ControllerMovementSpeed Speed { get; protected set; }
        public ControllerMovementDirection Direction { get; protected set; }

        public ControllerGroundCheck GroundCheck => Controller.GroundCheck;
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

            Speed = Dependancy.Get<ControllerMovementSpeed>(Controller.gameObject);
            Direction = Dependancy.Get<ControllerMovementDirection>(Controller.gameObject);
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
            Controller.OnFixedProcess += FixedProcess;
        }

        void Process()
        {
            CalculateInput();
        }

        public float Multiplier { get; protected set; }

        void FixedProcess()
        {
            Gravity.Apply();
            GroundCheck.Do();

            if (GroundCheck.IsGrounded)
                Multiplier = State.Multiplier;

            Speed.Calculate(Multiplier);

            Target = CalculateTarget();

            Velocity.Absolute = Vector3.MoveTowards(Velocity.Absolute, Target, acceleration * State.Multiplier * Time.deltaTime);

            Debug.DrawRay(Controller.transform.position, Target, Color.yellow);
            Debug.DrawRay(Controller.transform.position, Velocity.Absolute, Color.red);
        }

        protected virtual void CalculateInput()
        {
            Input = (Direction.Forward * Controller.Input.Move.y) + (Direction.Right * Controller.Input.Move.x);

            Input = Vector3.ClampMagnitude(Input, 1f);
        }

        protected virtual Vector3 CalculateTarget()
        {
            var result = Input * Speed.Value;

            if (GroundCheck.IsGrounded)
                result = Vector3.ProjectOnPlane(result, GroundCheck.Hit.Normal);

            result += Controller.Velocity.Calculate(Gravity.Direction);

            return result;
        }
    }
}