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
	public class WeaponInputLockOnEnable : Weapon.Module, WeaponConstraint.IInterface
	{
        public bool Active { get; protected set; }

        bool WeaponConstraint.IInterface.Constraint => Active;

        protected virtual void OnEnable()
        {
            Begin();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        protected virtual void Begin()
        {
            Active = true;
        }

        protected virtual void Process()
        {
            if(Active)
            {
                if (enabled)
                {
                    if (Weapon.Action.Input == false) Stop();
                }
                else
                    Stop();
            }
        }

        protected virtual void End()
        {
            Stop();
        }

        public virtual void Stop()
        {
            Active = false;
        }
    }
}