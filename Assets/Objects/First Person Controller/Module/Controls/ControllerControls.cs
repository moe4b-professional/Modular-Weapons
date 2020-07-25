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

        public AxisInput Lean { get; protected set; }

        public ControllerInput Input { get; protected set; }
        public ControllerInput.Context Context => Input.Current;

        public override void Configure()
        {
            base.Configure();

            Move = new AxesInput();
            Look = new AxesInput();

            Jump = new ButtonInput();

            Sprint = new SingleAxisInput();

            Crouch = new ButtonInput();
            Prone = new ButtonInput();

            Lean = new AxisInput();

            Input = Controller.Modules.Find<ControllerInput>();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        protected virtual void Process()
        {
            Input.Process();

            Move.Process(Context.Move);
            Look.Process(Context.Look.Value);

            Jump.Process(Context.Jump);

            Sprint.Process(Context.Sprint);

            Crouch.Process(Context.Crouch);
            Prone.Process(Context.Prone);

            Lean.Process(Context.Lean);
        }
    }
}