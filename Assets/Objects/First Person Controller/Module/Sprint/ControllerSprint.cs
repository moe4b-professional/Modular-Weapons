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

        public SingleAxisInput Input => Controller.Controls.Sprint;

        public Modules<ControllerSprint> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerSprint>
        {
            public ControllerSprint Sprint { get; protected set; }
            public virtual void Set(ControllerSprint value) => Sprint = value;

            public FirstPersonController Controller => Sprint.Controller;
        }

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Modules = new Modules<ControllerSprint>(this);
            Modules.Register(Controller.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Constraint = new Modifier.Constraint();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
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
                        Target = Input.Value;
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