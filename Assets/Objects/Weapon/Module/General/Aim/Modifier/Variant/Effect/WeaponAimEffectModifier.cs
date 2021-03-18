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

        public List<Element> Elements { get; protected set; }
        [Serializable]
        public class Element
        {
            public WeaponAimEffectModifier EffectModifier { get; protected set; }

            public WeaponEffects.IInterface Target { get; protected set; }

            public float Value => EffectModifier.Value;

            public float Modifier() => Value;

            protected virtual void Register()
            {
                Target.Scale.Add(Modifier);
            }

            public Element(WeaponAimEffectModifier modifier, WeaponEffects.IInterface target)
            {
                this.EffectModifier = modifier;

                this.Target = target;

                Register();
            }
        }

        public virtual float Value => Mathf.Lerp(range.Max, range.Min, Rate);

        public WeaponEffects Effects => Weapon.Effects;

        public override void Configure()
        {
            base.Configure();

            Elements = new List<Element>();

            Effects.OnRegister += EffectRegisterCallback;
        }

        protected virtual void EffectRegisterCallback(WeaponEffects.IInterface effect)
        {
            if (IsTarget(effect)) Register(effect);
        }

        protected virtual void Register(WeaponEffects.IInterface effect)
        {
            var element = new Element(this, effect);

            Elements.Add(element);
        }

        public abstract bool IsTarget(WeaponEffects.IInterface effect);
    }
}