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
	public class WeaponAim : Weapon.Module
	{
        public bool IsOn { get; protected set; }

        public float Target => IsOn ? 1f : 0f;

        private float _rate;
        public float Rate
        {
            get => _rate;
            set
            {
                if (value == _rate) return;

                _rate = value;

                Processor.Rate = Rate;

                OnRateChange?.Invoke(Rate);
            }
        }
        public delegate void RateChangeDelegate(float rate);
        public event RateChangeDelegate OnRateChange;

        public Modifier.Constraint Constraint { get; protected set; }

        public bool CanPerform
        {
            get
            {
                if (Constraint.Active) return false;

                return true;
            }
        }

        public WeaponAimSpeed Speed { get; protected set; }

        public Modules<WeaponAim> Modules { get; protected set; }

        public abstract class Module : Weapon.Behaviour, IModule<WeaponAim>
        {
            public WeaponAim Aim { get; protected set; }
            public virtual void Set(WeaponAim value) => Aim = value;

            public Weapon Weapon => Aim.Weapon;
        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            bool Input { get; }

            float Rate { set; }

            void ClearInput();
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponAim>(this);
            Modules.Register(Weapon.Behaviours, ModuleScope.Global);

            Speed = Modules.Depend<WeaponAimSpeed>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>();

            Constraint = new Modifier.Constraint();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;
        }

        void DisableCallback()
        {
            Rate = 0f;
        }

        void Process()
        {
            if(Processor.Input)
            {
                if (CanPerform)
                    IsOn = Processor.Input;
                else
                    IsOn = false;
            }
            else
            {
                IsOn = false;
            }

            if (Rate != Target)
                Rate = Mathf.MoveTowards(Rate, Target, Speed.Value * Time.deltaTime);
        }

        public virtual void ClearInput() => Processor.ClearInput();
    }
}