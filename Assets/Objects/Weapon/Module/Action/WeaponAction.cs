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

        public WeaponActionInput Input { get; protected set; }

        public abstract class Module : Weapon.BaseModule<WeaponAction>
        {
            public WeaponAction Action => Reference;

            public override Weapon Weapon => Action.Weapon;
        }
        public Modules.Collection<WeaponAction> Modules { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : IContext
        {
            
        }

        public interface IContext
        {
            float Input { get; }
        }
        public IContext Context
        {
            get
            {
                if (Override.Active) return Override.Value;

                return Processor;
            }
        }
        
        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Modules = new Modules.Collection<WeaponAction>(this);
            Modules.Register(Weapon.Behaviours);

            Override = Modules.Depend<WeaponActionOverride>();
            Input = Modules.Depend<WeaponActionInput>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.OnLateProcess += LateProcess;

            Modules.Init();
        }

        #region Process
        void Process()
        {
            Input.Process(Context);

            if (Input.Active)
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
        #endregion

        #region Late Process
        void LateProcess()
        {
            if (LatePerformCondition)
            {
                LatePerform();

                LatePerformCondition = false;
            }
        }

        bool LatePerformCondition;

        public event PerformDelegate OnLatePerform;
        public virtual void LatePerform()
        {
            OnLatePerform?.Invoke();
        }
        #endregion
    }
}