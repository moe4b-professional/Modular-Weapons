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
	public class ControllerControls : FirstPersonController.Module
	{
        public AxesInput Move { get; protected set; }
        public AxesInput Look { get; protected set; }

        public ButtonInput Jump { get; protected set; }

        public SingleAxisInput Sprint { get; protected set; }

        public ButtonInput Crouch { get; protected set; }
        public ButtonInput Prone { get; protected set; }

        public ChangeStanceProperty ChangeStance { get; protected set; }
        public class ChangeStanceProperty
        {
            public float Time { get; protected set; }

            public float HoldTime => 0.5f;

            public int Mode { get; protected set; }

            public void Process(bool input)
            {
                if (input)
                    Time += UnityEngine.Time.deltaTime;
                else
                    Time = 0f;

                if (input)
                {
                    if (Time < HoldTime)
                        Mode = 1;
                    else
                        Mode = 2;
                }
                else
                    Mode = 0;
            }
        }

        public AxisInput Lean { get; protected set; }

        public List<ControllerInput> Inputs { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Inputs = Controller.Modules.FindAll<ControllerInput>();

            Move = new AxesInput();
            Look = new AxesInput();

            Jump = new ButtonInput();

            Sprint = new SingleAxisInput();

            ChangeStance = new ChangeStanceProperty();

            Crouch = new ButtonInput();
            Prone = new ButtonInput();

            Lean = new AxisInput();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        protected virtual void Process()
        {
            ProcessMove();
            ProcessLook();

            ProcessJump();

            ProcessSprint();

            ProcessChangeStance();
            ProcessCrouch();
            ProcessProne();

            ProcessLean();
        }

        protected virtual void ProcessMove()
        {
            var value = AddAll(Inputs, GetElement);

            Vector2 GetElement(ControllerInput input) => input.Move;

            Move.Process(value);
        }
        protected virtual void ProcessLook()
        {
            var value = AddAll(Inputs, GetElement);

            Vector2 GetElement(ControllerInput input) => input.Look.Value;

            Look.Process(value);
        }

        protected virtual void ProcessJump()
        {
            var value = AddAll(Inputs, GetElement);

            bool GetElement(ControllerInput input) => input.Jump;

            Jump.Process(value);
        }

        protected virtual void ProcessSprint()
        {
            var value = AddAll(Inputs, GetElement);

            float GetElement(ControllerInput input) => input.Sprint;

            Sprint.Process(value);
        }

        protected virtual void ProcessChangeStance()
        {
            var value = AddAll(Inputs, GetElement);

            bool GetElement(ControllerInput input) => input.ChangeStance;

            ChangeStance.Process(value);
        }

        protected virtual void ProcessCrouch()
        {
            var value = AddAll(Inputs, GetElement);

            bool GetElement(ControllerInput input) => input.Crouch;

            value |= ChangeStance.Mode == 1;

            Crouch.Process(value);
        }

        protected virtual void ProcessProne()
        {
            var value = AddAll(Inputs, GetElement);

            bool GetElement(ControllerInput input) => input.Prone;

            value |= ChangeStance.Mode == 2;

            Prone.Process(value);
        }

        protected virtual void ProcessLean()
        {
            var value = AddAll(Inputs, GetElement);

            float GetElement(ControllerInput input) => input.Lean;

            Lean.Process(value);
        }

        public static TResult AddAll<TSource, TResult>(IList<TSource> list, Func<TSource, TResult> func)
        {
            dynamic value = default(TResult);

            var type = typeof(TResult);

            for (int i = 0; i < list.Count; i++)
            {
                dynamic instance = func(list[i]);

                if (type == typeof(int)) value += instance;

                if (type == typeof(float)) value += instance;

                if (type == typeof(Vector2)) value += instance;

                if (type == typeof(Vector3)) value += instance;

                if (type == typeof(bool)) value |= instance;
            }

            return value;
        }
    }
}