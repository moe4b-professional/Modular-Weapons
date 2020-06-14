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
	public class PlayerInput : Player.Module
	{
		public ButtonInput Primary { get; protected set; }

		public ButtonInput Secondary { get; protected set; }

        public ButtonInput Reload { get; protected set; }

        public ButtonInput SwitchActionMode { get; protected set; }

        public ButtonInput SwitchSight { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Primary = new ButtonInput();

            Secondary = new ButtonInput();

            Reload = new ButtonInput();

            SwitchActionMode = new ButtonInput();

            SwitchSight = new ButtonInput();
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;
        }

        void Process()
        {
            Primary.Process(Input.GetKey(KeyCode.Mouse0));

            Secondary.Process(Input.GetKey(KeyCode.Mouse1));

            Reload.Process(Input.GetKey(KeyCode.R));

            SwitchActionMode.Process(Input.GetKey(KeyCode.B));

            SwitchSight.Process(Input.GetKey(KeyCode.Mouse2) || Input.GetKey(KeyCode.T));
        }
    }
}