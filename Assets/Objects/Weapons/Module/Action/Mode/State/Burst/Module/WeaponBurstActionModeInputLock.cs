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

        bool WeaponConstraint.IInterface.Constraint => Active;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Burst.OnEnd += EndCallback;
        }

        void Process()
        {
            if (Processor.Input == false || enabled == false) Active = false;
        }

        void EndCallback()
        {
            if(enabled) Active = true;
        }
    }
}