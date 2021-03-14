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

        [SerializeField]
        protected OverridesProperty overrides;
        public OverridesProperty Overrides { get { return overrides; } }
        [Serializable]
        public class OverridesProperty
        {
            [SerializeField]
            protected List<WeaponAimEffectModifier> list;
            public List<WeaponAimEffectModifier> List { get { return list; } }

            public WeaponAimEffectModifier Modifier { get; protected set; }
            public virtual void Configure(WeaponAimEffectModifier reference)
            {
                Modifier = reference;

                RegisterChildern();
            }

            protected virtual void RegisterChildern()
            {
                var childern = Dependancy.GetAll<WeaponAimEffectModifier>(Modifier.gameObject, Dependancy.Scope.Childern);

                for (int i = 0; i < childern.Count; i++)
                {
                    if (Contains(childern[i])) continue;

                    Add(childern[i]);
                }
            }

            public virtual void Add(WeaponAimEffectModifier element)
            {
                if (Contains(element))
                {
                    Debug.LogWarning("Trying to add duplicate " + typeof(WeaponAimEffectModifier).Name + ": " + element.name);
                    return;
                }

                list.Add(element);
            }

            public virtual bool Contains(WeaponAimEffectModifier target) => list.Contains(target);

            public virtual bool Detect(WeaponEffects.IInterface target)
            {
                for (int i = 0; i < list.Count; i++)
                    if (list[i].IsTarget(target))
                        return true;

                return false;
            }
        }

        public List<Element> Elements { get; protected set; }
        [Serializable]
        public class Element
        {
            public WeaponAimEffectModifier EffectModifier { get; protected set; }

            public WeaponEffects.IInterface Target { get; protected set; }

            public bool Overriden => EffectModifier.Overrides.Detect(Target);

            public float Value => Overriden ? 1f : EffectModifier.Value;

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

            overrides.Configure(this);

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