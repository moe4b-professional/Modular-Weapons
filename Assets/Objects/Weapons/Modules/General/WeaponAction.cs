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
	public class WeaponAction : Weapon.Module
	{
        public WeaponConstraint Constraint => Weapon.Constraint;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data.Input)
            {
                if (Constraint.Active)
                {

                }
                else
                {
                    Perform();
                }
            }
        }

        public delegate void PerformDelegate();
        public event PerformDelegate OnPerform;
        public virtual void Perform()
        {
            OnPerform?.Invoke();
        }
    }
}