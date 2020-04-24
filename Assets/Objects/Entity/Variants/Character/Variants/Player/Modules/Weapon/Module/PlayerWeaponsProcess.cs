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
        public ButtonInput PrimaryButton { get; protected set; }
        public bool PrimaryInput => PrimaryButton.Held;

        public ButtonInput SecondaryButton { get; protected set; }

        public ButtonInput ReloadButton { get; protected set; }

        public Vector2 Sway => Player.Look.Vector * Player.Look.Sensitivity;

        public override void Configure(Player reference)
        {
            base.Configure(reference);

            PrimaryButton = new ButtonInput();

            SecondaryButton = new ButtonInput();

            ReloadButton = new ButtonInput();
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;
        }

        void Process()
        {
            PrimaryButton.Process(Input.GetMouseButton(0));

            SecondaryButton.Process(Input.GetMouseButton(1));

            ReloadButton.Process(Input.GetKey(KeyCode.R));
        }

        public interface IData : Weapon.IProcessData
        {
            ButtonInput PrimaryButton { get; }

            ButtonInput SecondaryButton { get; }

            ButtonInput ReloadButton { get; }

            Vector2 Sway { get; }
        }
    }
}