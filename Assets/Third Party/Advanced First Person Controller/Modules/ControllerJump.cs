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
	public class ControllerJump : FirstPersonController.Module
	{
		[SerializeField]
        protected float force = 5f;
        public float Force { get { return force; } }

        [SerializeField]
        protected ForceMode mode = ForceMode.VelocityChange;
        public ForceMode Mode { get { return mode; } }

        [SerializeField]
        protected Vector3 direction = Vector3.up;
        public Vector3 Direction { get { return direction; } }

        public ButtonInput Button { get; protected set; }

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Button = new ButtonInput();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Button.Process(Controller.Input.Jump);

            if(Button.Press)
            {
                var vector = Controller.transform.TransformDirection(direction) * force;

                Controller.rigidbody.AddForce(vector, mode);
            }
        }
    }
}