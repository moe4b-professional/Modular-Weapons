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

            Weapon.OnLateProcess += LateProcess;
        }

        void LateProcess(Weapon.IProcessData data)
        {
            if(LatePerformCondition)
            {
                LatePerform();

                LatePerformCondition = false;
            }
        }

        public IOverride Override { get; set; }
        public interface IOverride
        {
            bool Input { get; }
        }

        void Process(Weapon.IProcessData data)
        {
            var input = Override == null ? data.Input : Override.Input;

            if (input)
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

            LatePerformCondition = true;
        }

        bool LatePerformCondition;

        public event PerformDelegate OnLatePerform;
        public virtual void LatePerform()
        {
            OnLatePerform?.Invoke();
        }
    }
}