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
	public class WeaponBurstActionModeInputLock : WeaponBurstActionMode.Module, WeaponConstraint.IInterface
    {
        public bool Active { get; protected set; }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;

            Burst.OnEnd += EndCallback;
        }

        void Process()
        {
            if (Weapon.Action.Input.Active == false || enabled == false) Active = false;
        }

        void EndCallback()
        {
            if(enabled) Active = true;
        }
    }
}