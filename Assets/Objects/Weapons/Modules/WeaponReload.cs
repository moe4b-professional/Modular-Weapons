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
	public class WeaponReload : Weapon.Module
	{
        [SerializeField]
        protected bool auto;
        public bool Auto { get { return auto; } }

        public WeaponAmmo Ammo { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Ammo = Weapon.GetComponentInChildren<WeaponAmmo>();
        }

        public override void Init()
        {
            base.Init();

            if(Ammo == null)
            {
                Debug.LogError(GetType().Name + " needs a " + nameof(WeaponAmmo) + " module to function, ignoring module");
                enabled = false;
                return;
            }

            Ammo.OnConsumption += ConsumptionCallback;
        }

        void ConsumptionCallback()
        {
            if(auto)
            {
                if(Ammo.Active)
                {
                    Ammo.Refill();
                }
            }
        }
    }
}