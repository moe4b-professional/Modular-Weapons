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
	public class PlayerWeaponsProcess : PlayerWeapons.Module, PlayerWeaponsProcess.IData
    {
        public ButtonInput PrimaryInput { get; protected set; }

        public ButtonInput SecondaryInput { get; protected set; }

        public override void Configure(Player reference)
        {
            base.Configure(reference);

            PrimaryInput = new ButtonInput();

            SecondaryInput = new ButtonInput();
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;
        }

        void Process()
        {
            PrimaryInput.Process(Input.GetMouseButton(0));

            SecondaryInput.Process(Input.GetMouseButton(1));
        }

        public interface IData : Weapon.IProcessData
        {
            ButtonInput SecondaryInput { get; }
        }
    }
}