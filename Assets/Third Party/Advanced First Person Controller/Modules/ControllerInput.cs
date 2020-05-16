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
                x = Input.GetAxisRaw("Look X"),
                y = Input.GetAxisRaw("Look Y")
            };
        }
    }
}