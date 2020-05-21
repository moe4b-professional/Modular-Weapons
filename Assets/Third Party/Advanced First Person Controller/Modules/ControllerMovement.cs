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
        protected float speed = 3f;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float acceleration = 15f;
        public float Acceleration { get { return acceleration; } }

        public Vector3 Target { get; protected set; }

        public ControllerGroundCheck GroundCheck => Controller.GroundCheck;
        public ControllerGravity Gravity => Controller.Gravity;

        public Vector3 Forward => Controller.transform.forward;
        public Vector3 Right => Controller.transform.right;
        public Vector3 Up => Controller.transform.up;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
            Controller.OnFixedProcess += FixedProcess;
        }

        void Process()
        {

        }

        void FixedProcess()
        {
            GroundCheck.Do(Target);

            Gravity.Apply();

            var velocity = Controller.rigidbody.velocity;

            Target = CalculateTarget();

            velocity = Vector3.MoveTowards(velocity, Target, acceleration * Time.deltaTime);

            Controller.rigidbody.velocity = velocity;

            Debug.DrawRay(Controller.transform.position, Target, Color.yellow);
            Debug.DrawRay(Controller.transform.position, velocity, Color.red);
        }

        protected virtual Vector3 CalculateTarget()
        {
            var input = Forward * Controller.Input.Move.y + Right * Controller.Input.Move.x;

            var result = Vector3.ClampMagnitude(input, 1) * speed * Controller.State.Multiplier;

            if (GroundCheck.IsGrounded)
                result = Vector3.ProjectOnPlane(result, GroundCheck.Hit.Normal);

            result += Controller.Velocity.Calculate(Gravity.Direction);

            return result;
        }
    }
}