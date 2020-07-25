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
	public class PlayerControls : Player.Module
	{
        public SingleAxisInput Primary { get; protected set; }

        public SingleAxisInput Secondary { get; protected set; }

        public ButtonInput Reload { get; protected set; }

        public AxisInput SwitchWeapon { get; protected set; }

        public ButtonInput SwitchActionMode { get; protected set; }

        public ButtonInput SwitchSight { get; protected set; }
        
        public List<PlayerInput> Inputs { get; protected set; }

        public PlayerInput Input { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Inputs = Player.Behaviours.FindAll<PlayerInput>();

            Primary = new SingleAxisInput();
            Secondary = new SingleAxisInput();

            Reload = new ButtonInput();

            SwitchWeapon = new AxisInput();
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
            ProcessInput();

            Primary.Process(Input.Primary);
            Secondary.Process(Input.Secondary);

            Reload.Process(Input.Reload);

            SwitchWeapon.Process(Input.SwitchWeapon);
            SwitchActionMode.Process(Input.SwitchActionMode);
            SwitchSight.Process(Input.SwitchSight);
        }

        protected virtual void ProcessInput()
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                if(Inputs[i].AnyInput)
                {
                    Input = Inputs[i];
                    continue;
                }
            }

            if (Input == null) Input = Inputs[0];
        }
    }
}