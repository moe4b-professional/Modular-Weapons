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

        public override void Init()
        {
            base.Init();

            Ammo.OnConsumption += ConsumptionCallback;
        }

        void ConsumptionCallback()
        {
            if(enabled && Ammo.CanConsume == false)
                Weapon.OnProcess += Process;

            void Process()
            {
                if (Reload.CanPerform && Weapon.Action.Input == false)
                {
                    Weapon.OnProcess -= Process;

                    Reload.Perform();
                }
            }
        }
    }
}