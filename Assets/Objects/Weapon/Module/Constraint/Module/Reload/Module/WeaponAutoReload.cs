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
	public class WeaponAutoReload : WeaponReload.Module
	{
        public WeaponAmmo Ammo => Reload.Ammo;

        public bool Lock { get; protected set; }

        public bool CanPerform
        {
            get
            {
                if (Lock) return false;

                if (Weapon.Action.Input) return false;

                if (Ammo.CanConsume) return false;

                if (Reload.CanPerform == false) return false;

                return true;
            }
        }

        public override void Configure()
        {
            base.Configure();

            Lock = false;
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Ammo.OnConsumption += AmmoConsumptionCallback;

            Reload.OnPerform += ReloadPerformCallback;
        }
        
        void Process()
        {
            if(enabled)
            {
                if (CanPerform)
                    Reload.Perform();
            }
        }

        void AmmoConsumptionCallback()
        {
            if (Ammo.CanConsume == false)
                Lock = false;
        }

        void ReloadPerformCallback()
        {
            Lock = true;
        }
    }
}