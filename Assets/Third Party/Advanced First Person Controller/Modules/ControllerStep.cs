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
        public float Rate { get; protected set; }

        [SerializeField]
        protected float scale;
        public float Scale { get { return scale; } }

        [SerializeField]
        protected float resetSpeed;
        public float ResetSpeed { get { return resetSpeed; } }

        public ControllerMovement Movement => Controller.Movement;
        public ControllerVelocity Velocity => Controller.Velocity;
        public ControllerState State => Controller.State;
        public ControllerGround Ground => Controller.Ground;
        public ControllerGravity Gravity => Controller.Gravity;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            var velocity = Velocity.Absolute;

            if (velocity.magnitude < 0.01f || Ground.IsGrounded == false)
            {
                if (Rate > 0.5f) Rate = Mathf.InverseLerp(1f, 0f, Rate);

                Rate = Mathf.MoveTowards(Rate, 0f, resetSpeed * Time.deltaTime);
            }
            else
            {
                Rate += velocity.magnitude * scale * Time.deltaTime;

                if (Rate >= 1f)
                    Complete();
            }
        }

        public event Action OnComplete;
        protected virtual void Complete()
        {
            Rate = 0f;

            OnComplete?.Invoke();
        }
    }
}