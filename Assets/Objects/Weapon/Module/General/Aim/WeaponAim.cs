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
	public class WeaponAim : Weapon.Module, WeaponOperation.IInterface
	{
        [SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

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

                OnRateChange?.Invoke(Rate);
            }
        }
        public float InverseRate => Mathf.Lerp(1f, 0f, Rate);
        public delegate void RateChangeDelegate(float rate);
        public event RateChangeDelegate OnRateChange;

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
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Modules = new Modules.Collection<WeaponAim>(this, Weapon.gameObject);

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
            IsOn = Processor.Input;

            if (Weapon.Operation.Is(null))
            {
                if (IsOn) Begin();
            }
            else if (Weapon.Operation.Is(this))
            {
                if (IsOn == false) End();
            }
            else
            {
                IsOn = false;
            }

            if (Rate != Target)
                Rate = Mathf.MoveTowards(Rate, Target, speed * Time.deltaTime);
        }

        protected virtual void Begin()
        {
            Weapon.Operation.Set(this);
        }

        protected virtual void End()
        {
            Weapon.Operation.Clear();
        }

        public virtual void Stop()
        {
            End();
        }
    }
}