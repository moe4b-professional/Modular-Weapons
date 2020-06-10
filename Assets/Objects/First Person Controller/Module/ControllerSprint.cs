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
	public class ControllerSprint : FirstPersonController.Module, Modifier.Scale.IInterface
	{
        [SerializeField]
        protected float acceleration = 5f;
        public float Acceleration { get { return acceleration; } }

        public float Weight { get; protected set; }

        [SerializeField]
        protected float multiplier = 2f;
        public float Multiplier { get { return multiplier; } }

        [SerializeField]
        protected InputMode mode = InputMode.Toggle;
        public InputMode Mode { get { return mode; } }
        public enum InputMode
        {
            Toggle, Hold
        }

        public float Axis
        {
            get
            {
                switch (mode)
                {
                    case InputMode.Toggle:
                        return 1f;

                    case InputMode.Hold:
                        return Input.Axis;
                }

                throw new NotImplementedException();
            }
        }

        public ControllerInput.SprintInput Input => Controller.Input.Sprint;
        
        public virtual bool CanPerform
        {
            get
            {
                if (Vector3.Dot(Controller.Movement.Input.Relative, Vector3.forward) < 0.5f) return false;

                return true;
            }
        }

        float Modifier.Scale.IInterface.Value => Mathf.Lerp(1f, 2f, Weight) * multiplier;

        public override void Init()
        {
            base.Init();

            Controller.Movement.Speed.Scale.Register(this);

            Controller.OnProcess += Process;
        }

        private void Process()
        {
            
        }

        protected virtual void Begin()
        {
            
        }

        protected virtual void End()
        {
            
        }
    }
}