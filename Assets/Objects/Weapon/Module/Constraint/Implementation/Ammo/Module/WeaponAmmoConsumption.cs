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
	public class WeaponAmmoConsumption : WeaponAmmo.Module
	{
        public override void Initialize()
        {
            base.Initialize();

            Weapon.Action.OnLatePerform += LateAction;
        }

        void LateAction()
        {
            if(enabled)
            {
                if (Ammo.CanConsume)
                {
                    Ammo.Consume();
                }
                else
                {
                    Debug.LogWarning("Trying to consume ammo when there isn't enough ammo to be consumed");
                }
            }
        }
    }
}