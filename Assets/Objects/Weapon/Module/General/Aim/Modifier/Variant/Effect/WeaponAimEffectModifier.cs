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
	public abstract class WeaponAimEffectModifier : WeaponAimPropertyModifier
	{
        [SerializeField]
        protected ValueRange range = new ValueRange(0.3f, 1f);
        public ValueRange Range { get { return range; } }

        public List<WeaponEffects.IInterface> Targets { get; protected set; }

        public virtual float Value => Mathf.Lerp(range.Max, range.Min, Rate);

        float Modifier() => Value;

        public WeaponEffects Effects => Weapon.Effects;

        public override void Configure()
        {
            base.Configure();

            Targets = new List<WeaponEffects.IInterface>();

            Effects.OnRegister += EffectRegisterCallback;
        }

        protected virtual void EffectRegisterCallback(WeaponEffects.IInterface effect)
        {
            if (IsTarget(effect)) Register(effect);
        }

        public abstract bool IsTarget(WeaponEffects.IInterface effect);

        protected virtual void Register(WeaponEffects.IInterface effect)
        {
            if (Targets.Contains(effect))
            {
                Debug.LogWarning($"Duplicate Effect Registeration for {effect} in {this}, Ignored");
                return;
            }

            Targets.Add(effect);

            effect.Scale.Add(Modifier);
        }
    }
}