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
	public class ControllerSprint : FirstPersonController.Module
	{
        [SerializeField]
        protected float acceleration = 5f;
        public float Acceleration { get { return acceleration; } }

        [SerializeField]
        protected InputMode mode = InputMode.Toggle;
        public InputMode Mode { get { return mode; } }
        public enum InputMode
        {
            Toggle, Hold
        }

        public float Weight { get; protected set; }

        public float Target { get; protected set; }

        public bool Active => Target > 0f;

        public Modifier.Constraint Constraint { get; protected set; }

        public virtual bool CanPerform
        {
            get
            {
                if (Vector3.Dot(Controller.Movement.Input.Relative, Vector3.forward) < 0.5f) return false;

                if (Constraint.Active) return false;

                if (Active == false && Controller.IsGrounded == false) return false;

                return true;
            }
        }

        public ControllerInput.SprintInput Input => Controller.Input.Sprint;

        public class Module : FirstPersonController.BaseModule<ControllerSprint>
        {
            public ControllerSprint Sprint => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }

        public Modules.Collection<ControllerSprint> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Constraint = new Modifier.Constraint();

            Modules = new Modules.Collection<ControllerSprint>(this);
            Modules.Register(Controller.Behaviours);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            Modules.Init();
        }

        void Process()
        {
            if(Active)
            {
                if (CanPerform == false)
                    Stop();
            }

            Weight = Mathf.MoveTowards(Weight, Target, acceleration * Time.deltaTime);
        }

        public event Action OnOperate;
        public virtual void Operate()
        {
            if (mode == InputMode.Hold)
            {
                if (Input.Button.Held)
                {
                    if (CanPerform)
                        Target = Input.Axis;
                }
                else
                {
                    if (Active)
                        Stop();
                }
            }

            if (mode == InputMode.Toggle)
            {
                if (Input.Button.Press)
                {
                    if (Active)
                        Stop();
                    else
                        Begin();
                }
            }

            OnOperate?.Invoke();
        }

        public virtual void Begin()
        {
            Target = 1f;
        }

        public virtual void Stop()
        {
            Target = 0f;
        }
    }
}