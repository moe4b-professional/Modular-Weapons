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
            ProcessPrimary();
            ProcessSecondary();

            ProcessReload();

            ProcessSwitchWeapon();
            ProcessSwitchActionMode();
            ProcessSwitchSight();
        }

        protected virtual void ProcessPrimary()
        {
            var value = AddAll(Inputs, GetElement);

            float GetElement(PlayerInput input) => input.Primary;

            Primary.Process(value);
        }
        protected virtual void ProcessSecondary()
        {
            var value = AddAll(Inputs, GetElement);

            float GetElement(PlayerInput input) => input.Secondary;

            Secondary.Process(value);
        }

        protected virtual void ProcessReload()
        {
            var value = AddAll(Inputs, GetElement);

            bool GetElement(PlayerInput input) => input.Reload;

            Reload.Process(value);
        }

        protected virtual void ProcessSwitchWeapon()
        {
            var value = AddAll(Inputs, GetElement);

            float GetElement(PlayerInput input) => input.SwitchWeapon;

            SwitchWeapon.Process(value);
        }
        protected virtual void ProcessSwitchActionMode()
        {
            var value = AddAll(Inputs, GetElement);

            bool GetElement(PlayerInput input) => input.SwitchActionMode;

            SwitchActionMode.Process(value);
        }
        protected virtual void ProcessSwitchSight()
        {
            var value = AddAll(Inputs, GetElement);

            bool GetElement(PlayerInput input) => input.SwitchSight;

            SwitchSight.Process(value);
        }

        public static TResult AddAll<TSource, TResult>(IList<TSource> list, Func<TSource, TResult> func)
        {
            dynamic value = default(TResult);

            var type = typeof(TResult);

            for (int i = 0; i < list.Count; i++)
            {
                dynamic instance = func(list[i]);

                if (type == typeof(int)) value += instance;

                if (type == typeof(float)) value += instance;

                if (type == typeof(Vector2)) value += instance;

                if (type == typeof(Vector3)) value += instance;

                if (type == typeof(bool)) value |= instance;
            }

            return value;
        }
    }
}