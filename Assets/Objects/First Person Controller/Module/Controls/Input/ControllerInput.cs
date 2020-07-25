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
	public class ControllerInput : FirstPersonController.Module
	{
        public class Module : FirstPersonController.BaseModule<ControllerInput>
        {
            public ControllerInput ControllerInput => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }
        public Modules.Collection<ControllerInput> Modules { get; protected set; }

        public class Context : Module
        {
            public Vector2 Move { get; protected set; }
            
            [SerializeField]
            protected LookProperty look;
            public LookProperty Look { get { return look; } }

            public bool Jump { get; protected set; }

            public float Sprint { get; protected set; }

            public bool Crouch { get; protected set; }
            public bool Prone { get; protected set; }

            public float Lean { get; protected set; }

            public bool AnyInput
            {
                get
                {
                    if (Move.sqrMagnitude > 0f) return true;

                    if (look.Value.sqrMagnitude > 0f) return true;

                    if (Jump) return true;

                    if (Sprint > 0f) return true;

                    if (Crouch) return true;

                    if (Prone) return true;

                    if (Mathf.Abs(Lean) > 0f) return true;

                    return false;
                }
            }

            public override void Init()
            {
                base.Init();

                ControllerInput.OnProcess += Process;
            }

            protected virtual void Process()
            {
                
            }
        }
        public List<Context> Contexts { get; protected set; }

        public Context Current { get; protected set; }
        protected virtual void UpdateState()
        {
            for (int i = 0; i < Contexts.Count; i++)
            {
                if (Contexts[i].AnyInput)
                {
                    Current = Contexts[i];
                    break;
                }
            }

            if (Current == null) Current = Contexts[0];
        }

        [Serializable]
        public class LookProperty
        {
            [SerializeField]
            protected float sensitivity = 1f;
            public float Sensitivity { get { return sensitivity; } }

            public Vector2 Value { get; protected set; }
            public void SetValue(Vector2 input)
            {
                input *= sensitivity;

                Value = input;
                RawValue = Value / Time.deltaTime;
            }
            public void SetValue(float x, float y) => SetValue(new Vector2(x, y));

            public Vector2 RawValue { get; protected set; }
            public void SetRawValue(Vector2 input)
            {
                input *= sensitivity;

                RawValue = input;
                Value = RawValue * Time.deltaTime;
            }
            public void SetRawValue(float x, float y) => SetRawValue(new Vector2(x, y));
        }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<ControllerInput>(this);
            Modules.Register(Controller.Behaviours);
            Contexts = Modules.FindAll<Context>();
            Modules.Configure();

            UpdateState();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }

        public event Action OnProcess;
        public virtual void Process()
        {
            OnProcess?.Invoke();

            UpdateState();
        }

        public static float GetAxis(KeyCode positive, KeyCode negative)
        {
            if (Input.GetKey(positive)) return 1f;

            if (Input.GetKey(negative)) return -1f;

            return 0f;
        }
    }
}