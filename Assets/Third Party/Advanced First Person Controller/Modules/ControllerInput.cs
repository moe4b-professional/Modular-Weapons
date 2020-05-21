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
        public Vector2 Move { get; protected set; }

        public Vector2 Look { get; protected set; }

        public ButtonInput Jump { get; protected set; }

        public ButtonInput Sprint { get; protected set; }

        public ButtonInput Crouch { get; protected set; }

        public ButtonInput Prone { get; protected set; }

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Jump = new ButtonInput();

            Sprint = new ButtonInput();

            Crouch = new ButtonInput();

            Prone = new ButtonInput();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Move = new Vector2()
            {
                x = Input.GetAxisRaw("Move X"),
                y = Input.GetAxisRaw("Move Y")
            };

            Look = new Vector2()
            {
                x = Input.GetAxis("Look X"),
                y = Input.GetAxis("Look Y")
            };

            Jump.Process(Input.GetKey(KeyCode.Space));

            Sprint.Process(Input.GetKey(KeyCode.LeftShift));

            Crouch.Process(Input.GetKey(KeyCode.C));

            Prone.Process(Input.GetKey(KeyCode.LeftControl));
        }
    }
}