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
	public class WeaponSemiActionMode : Weapon.Module, WeaponConstraint.IInterface, WeaponActionMode.IState
    {
		public bool InputLock { get; protected set; }

        bool WeaponConstraint.IInterface.Constraint => InputLock;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Action.OnPerform += Action;
        }

        void Process(Weapon.IProcessData data)
        {
            if(enabled)
            {
                if (data.Input == false)
                    InputLock = false;
            }
            else
            {
                InputLock = false;
            }
        }

        void Action()
        {
            if (enabled) InputLock = true;
        }
    }
}