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
	public class WeaponSingleActionMode : Weapon.Module, WeaponConstraint.IInterface, WeaponActionMode.IState
    {
		public bool InputLock { get; protected set; }

        bool WeaponConstraint.IInterface.Active => InputLock;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Action.OnPerform += Action;
        }

        void Process()
        {
            if(enabled)
            {
                if (Weapon.Action.Input.Active == false)
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