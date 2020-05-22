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
	public class ControllerStep : FirstPersonController.Module
	{
        [SerializeField]
        protected float length = 1.5f;
        public float Length { get { return length; } }

        public float Value { get; protected set; }

        public ControllerMovement Movement => Controller.Movement;
        public ControllerVelocity Velocity => Controller.Velocity;
        public ControllerState State => Controller.State;
        public ControllerGroundCheck GroundCheck => Controller.GroundCheck;
        public ControllerGravity Gravity => Controller.Gravity;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            var velocity = Velocity.Absolute - Velocity.Calculate(Gravity.Direction);

            Value += velocity.magnitude * Time.deltaTime;

            if (Value > length)
            {
                Value = 0f;
            }
        }
    }
}