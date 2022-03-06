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

using MB;

namespace Game
{
	public class ControllerLookLean : ControllerLook.Module
	{
		[SerializeField]
        protected float range = 40f;
        public float Range { get { return range; } }

        [SerializeField]
        protected float speed = 3f;
        public float Speed { get { return speed; } }

        public float Target { get; protected set; }

        public float Rate { get; protected set; }

        public float Angle => range * Rate;

        [SerializeField]
        protected InputMode mode = InputMode.Hold;
        public InputMode Mode { get { return mode; } }
        public enum InputMode
        {
            Hold, Toggle
        }

        public Vector3 Axis => Vector3.forward;

        public Quaternion Offset { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<ControllerLookLean> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerLookLean>
        {
            [field: SerializeField, DebugOnly]
            public ControllerLookLean Lean { get; protected set; }

            public FirstPersonController Controller => Lean.Controller;

            public virtual void Set(ControllerLookLean value) => Lean = value;
        }

        public AxisInput Input => Controller.Controls.Lean;

        public override void Set(ControllerLook value)
        {
            base.Set(value);

            Modules = new Modules<ControllerLookLean>(this);
            Modules.Register(Controller.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Offset = Quaternion.identity;
        }

        public override void Initialize()
        {
            base.Initialize();

            //CalculateOffset(); //TODO Remove if Unecessary

            Controller.OnProcess += Process;
        }

        void Process()
        {
            CalculateTarget();

            Rate = Mathf.MoveTowards(Rate, Target, speed * Time.deltaTime);

            CalculateOffset();
        }

        protected virtual void CalculateTarget()
        {
            ProcessButton(Input.Positive, 1f);

            ProcessButton(Input.Negative, -1f);
        }

        protected virtual void ProcessButton(ButtonInput button, float value)
        {
            if(mode == InputMode.Hold)
            {
                if (button.Click)
                    Target = value;
                if (button.Lift)
                    Target = 0f;
            }

            if(mode == InputMode.Toggle)
            {
                if(button.Click)
                {
                    if (Target == value)
                        Target = 0f;
                    else if (Target == 0f)
                        Target = value;
                    else
                        Target = 0f;
                }
            }
        }

        public virtual void Stop()
        {
            Target = 0f;
        }

        protected virtual void CalculateOffset()
        {
            Offset = Quaternion.Euler(Axis * -Angle);
        }
    }
}