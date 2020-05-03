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
	public class WeaponFiringModeSelection : Weapon.Module
	{
        [SerializeField]
        protected WeaponActionMode.Enum[] selection = new WeaponActionMode.Enum[] { WeaponActionMode.Enum.Semi, WeaponActionMode.Enum.Auto };
        public WeaponActionMode.Enum[] Selection { get { return selection; } }

        public int Index { get; protected set; }

        public WeaponActionMode Module { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Module = Weapon.GetComponentInChildren<WeaponActionMode>();
        }

        public override void Init()
        {
            base.Init();

            if(Module == null)
            {
                Debug.Log(FormatDependancyError<WeaponActionMode>());
                enabled = false;
                return;
            }

            Index = Array.IndexOf(selection, Module.Mode);

            if(Index < 0)
            {
                Index = 0;
                Module.Mode = selection[Index];
            }

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData) Process(data as IData);
        }
        void Process(IData data)
        {
            if (data.Input)
                Change();
        }

        public delegate void ChangeDelegate(WeaponActionMode.Enum mode);
        public event ChangeDelegate OnChange;
        protected virtual void Change()
        {
            Index++;

            if (Index >= selection.Length) Index = 0;

            Module.Mode = selection[Index];

            OnChange?.Invoke(Module.Mode);
        }

        public interface IData
        {
            bool Input { get; }
        }
    }
}