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

using MB;

namespace Game
{
	public class WeaponAction : Weapon.Module
	{
        [field: SerializeField, DebugOnly]
        public WeaponActionOverride Override { get; protected set; }

        [field: SerializeField, DebugOnly]
        public WeaponActionInput Input { get; protected set; }

        [field: SerializeField, DebugOnly]
        public WeaponActionControl Control { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<WeaponAction> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponAction>
        {
            [field: SerializeField, DebugOnly]
            public WeaponAction Action { get; protected set; }

            public Weapon Weapon => Action.Weapon;

            public virtual void Set(WeaponAction value) => Action = value;
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

        public WeaponConstraint Constraint => Weapon.Constraint;

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

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;

            Weapon.OnLateProcess += LateProcess;
        }

        #region Process
        void Process()
        {
            Input.Process(Context);

            if (Control.Active)
                Perform();
            else
                Idle();
        }

        public delegate void PerformDelegate();
        public event PerformDelegate OnPerform;
        public virtual void Perform()
        {
            OnPerform?.Invoke();

            LatePerformCondition = true;
        }

        public delegate void IdleDelegate();
        public event IdleDelegate OnIdle;
        public virtual void Idle()
        {
            OnIdle?.Invoke();
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