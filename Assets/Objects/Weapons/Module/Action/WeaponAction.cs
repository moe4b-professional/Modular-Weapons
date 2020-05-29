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

        public WeaponActionOverride Override { get; protected set; }

        public class Module : Weapon.BaseModule<WeaponAction>
        {
            public WeaponAction Action => Reference;

            public override Weapon Weapon => Action.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Override = Dependancy.Get<WeaponActionOverride>(gameObject);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.OnLateProcess += LateProcess;

            References.Init(this);
        }

        void Process()
        {
            var input = CalculateInput(Processor);

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

        void LateProcess()
        {
            if (LatePerformCondition)
            {
                LatePerform();

                LatePerformCondition = false;
            }
        }

        protected virtual bool CalculateInput(Weapon.IProcessor data)
        {
            if (Override.Active)
                return Override.Value.Input;

            return data.Input;
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