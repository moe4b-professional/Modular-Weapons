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
	public class WeaponPlayerReload : Weapon.Module
	{
		public WeaponReload Reload { get; protected set; }

        public WeaponAmmo Ammo => Reload.Ammo;

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Reload = Weapon.GetComponentInChildren<WeaponReload>();
        }

        public override void Init()
        {
            base.Init();

            if(Reload == null)
            {
                Debug.LogError(GetType().Name + " needs a " + nameof(WeaponReload) + " module to function, ignoring module");
                enabled = false;
                return;
            }

            if(Ammo == null)
            {
                Debug.LogError(GetType().Name + " needs a " + nameof(WeaponAmmo) + " module to function, ignoring module");
                enabled = false;
                return;
            }

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is PlayerWeaponsProcess.IData)
                Process(data as PlayerWeaponsProcess.IData);
        }

        void Process(PlayerWeaponsProcess.IData data)
        {
            if(data.ReloadButton.Press)
            {
                Ammo.Refill();
            }
        }
    }
}