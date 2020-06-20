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
	public class WeaponAimEffectModifier : WeaponAimPropertyModifier
	{
        public abstract class Context : Module, Modifier.Scale.IInterface
        {
            [SerializeField]
            protected ValueRange range = new ValueRange(0.3f, 1f);
            public ValueRange Range { get { return range; } }

            public abstract IList<WeaponEffects.IInterface> Targets { get; }

            public virtual float Value => Mathf.Lerp(range.Max, range.Min, Effects.Rate);

            public override void Init()
            {
                base.Init();

                Register();
            }

            protected virtual void Register()
            {
                for (int i = 0; i < Targets.Count; i++)
                    Targets[i].Scale.Register(this);
            }
        }

        public abstract class Module : Weapon.BaseModule<WeaponAimEffectModifier>
        {
            public WeaponAimEffectModifier Effects => Reference;

            public WeaponAim Aim => Reference.Aim;

            public override Weapon Weapon => Reference.Weapon;
        }

        public Modules.Collection<WeaponAimEffectModifier> Modules { get; protected set; }

        public virtual float Value => Rate;

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<WeaponAimEffectModifier>(this, Weapon.gameObject);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();

            Aim.OnRateChange += RateChangeCallback;

            UpdateState();
        }

        void RateChangeCallback(float rate) => UpdateState();

        public event Action OnUpdateState;
        protected virtual void UpdateState()
        {
            OnUpdateState?.Invoke();
        }
    }
}