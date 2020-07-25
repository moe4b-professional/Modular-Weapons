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

        public ControllerInput Input { get; protected set; }

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
            ProcessInput();

            Move.Process(Input.Move);
            Look.Process(Input.Look.Value);

            Jump.Process(Input.Jump);

            Sprint.Process(Input.Sprint);

            ChangeStance.Process(Input.ChangeStance);

            Crouch.Process(Input.Crouch | ChangeStance.Mode == 1);
            Prone.Process(Input.Prone | ChangeStance.Mode == 2);
            
            Lean.Process(Input.Lean);
        }

        protected virtual void ProcessInput()
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                if (Inputs[i].AnyInput)
                {
                    Input = Inputs[i];
                    continue;
                }
            }

            if (Input == null) Input = Inputs[0];
        }
    }
}