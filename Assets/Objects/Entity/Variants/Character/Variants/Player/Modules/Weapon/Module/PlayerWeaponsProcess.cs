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
    public class PlayerWeaponsProcess : PlayerWeapons.Module, Weapon.IProcessData,
        WeaponAim.IData, WeaponSway.IData, WeaponReload.IData, WeaponBob.IData
    {
        public ButtonInput PrimaryButton { get; protected set; }
        bool Weapon.IProcessData.Input => PrimaryButton.Held;

        public ButtonInput SecondaryButton { get; protected set; }

        [SerializeField]
        protected AimData aim;
        public AimData Aim { get { return aim; } }
        [Serializable]
        public class AimData
        {
            [SerializeField]
            protected InputAggregationMode mode = InputAggregationMode.Toggle;
            public InputAggregationMode Mode { get { return mode; } }

            public bool Input { get; protected set; }

            public virtual void Process(ButtonInput button)
            {
                if (mode == InputAggregationMode.Hold)
                {
                    Input = button.Held;
                }
                else if(mode == InputAggregationMode.Toggle)
                {
                    if (button.Press) Input = !Input;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        bool WeaponAim.IData.Input => Aim.Input;

        public ButtonInput ReloadButton { get; protected set; }
        bool BaseWeaponReload.IData.Input => ReloadButton.Press;

        public Vector2 Sway => Player.Look.Vector * Player.Look.Sensitivity;
        Vector2 WeaponSway.IData.Value => Sway;

        Vector3 WeaponBob.IData.Velocity => Player.rigidbody.velocity;

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
            PrimaryButton.Process(UnityEngine.Input.GetMouseButton(0));

            SecondaryButton.Process(UnityEngine.Input.GetMouseButton(1));

            ReloadButton.Process(UnityEngine.Input.GetKey(KeyCode.R));

            aim.Process(SecondaryButton);
        }
    }
}