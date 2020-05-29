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
	public class WeaponAim : Weapon.Module<WeaponAim.IProcessor>, WeaponOperation.IInterface
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

        public class Module : Weapon.BaseModule<WeaponAim>
        {
            public WeaponAim Aim => Reference;

            public override Weapon Weapon => Aim.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            References.Configure(this, Weapon.gameObject);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;

            References.Init(this, Weapon.gameObject);
        }

        void DisableCallback()
        {
            Rate = 0f;
        }

        void Process()
        {
            if (HasProcessor) Process(Processor);
        }
        protected virtual void Process(IProcessor data)
        {
            IsOn = data.Input;

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

        public interface IProcessor
        {
            bool Input { get; }
        }
    }
}