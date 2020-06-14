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

        public abstract class Module : Weapon.BaseModule<WeaponAim>
        {
            public WeaponAim Aim => Reference;

            public override Weapon Weapon => Aim.Weapon;
        }

        public Modules.Collection<WeaponAim> Modules { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            bool Input { get; }

            float Rate { set; }

            void ClearInput();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Constraint = new Modifier.Constraint();

            Modules = new Modules.Collection<WeaponAim>(this, Weapon.gameObject);

            Speed = Modules.Depend<WeaponAimSpeed>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;

            Modules.Init();
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