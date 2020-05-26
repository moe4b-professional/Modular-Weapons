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
        WeaponAim.IData, WeaponSway.IData, WeaponReload.IData, WeaponBob.IData, WeaponActionMode.IData, WeaponSprint.IData
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
        bool WeaponReload.IData.Input => ReloadButton.Press;

        Vector2 WeaponSway.IData.Look => Player.Controller.Look.Delta;
        Vector3 WeaponSway.IData.RelativeVelocity => Player.Controller.Velocity.Relative;

        float WeaponBob.IData.Step => Player.Controller.Step.Rate;

        public ButtonInput SwitchFireModeButton { get; protected set; }
        bool WeaponActionMode.IData.Switch => SwitchFireModeButton.Press;

        float WeaponSprint.IData.Weight => Player.Controller.State.Sets.Sprint.Weight;

        public override void Configure(PlayerWeapons reference)
        {
            base.Configure(reference);

            PrimaryButton = new ButtonInput();

            SecondaryButton = new ButtonInput();

            ReloadButton = new ButtonInput();

            SwitchFireModeButton = new ButtonInput();
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

            SwitchFireModeButton.Process(Input.GetKey(KeyCode.B));
        }
    }
}