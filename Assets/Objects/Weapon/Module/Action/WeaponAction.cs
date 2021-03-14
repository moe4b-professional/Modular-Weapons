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

        public WeaponActionControl Control { get; protected set; }

        public Modules<WeaponAction> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponAction>
        {
            public WeaponAction Action { get; protected set; }
            public virtual void Set(WeaponAction value) => Action = value;

            public Weapon Weapon => Action.Weapon;
        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor, IContext
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

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponAction>(this);
            Modules.Register(Weapon.Behaviours);

            Override = Modules.Depend<WeaponActionOverride>();
            Input = Modules.Depend<WeaponActionInput>();
            Control = Modules.Depend<WeaponActionControl>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.OnLateProcess += LateProcess;
        }

        #region Process
        void Process()
        {
            Input.Process(Context);

            if (Control.Active)
                Perform();
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