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
        public abstract class Context : Module
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
                protected Context[] list;
                public Context[] List { get { return list; } }

                public virtual bool Contains(WeaponEffects.IInterface target)
                {
                    for (int x = 0; x < list.Length; x++)
                        for (int y = 0; y < list[x].Targets.Count; y++)
                            if (list[x].Targets[y] == target)
                                return true;

                    return false;
                }
            }

            public abstract IList<WeaponEffects.IInterface> Targets { get; }

            public List<Element> Elements { get; protected set; }
            [Serializable]
            public class Element : Modifier.Scale.IInterface
            {
                public Context Context { get; protected set; }

                public WeaponEffects.IInterface Target { get; protected set; }

                public bool Override => Context.Overrides.Contains(Target);

                public float Value => Override ? 1f : Context.Value;

                public virtual void Register()
                {
                    Target.Scale.Register(this);
                }

                public Element(Context context, WeaponEffects.IInterface target)
                {
                    this.Context = context;

                    this.Target = target;
                }
            }

            public virtual float Value => Mathf.Lerp(range.Max, range.Min, Effects.Rate);

            public override void Init()
            {
                base.Init();

                Elements = new List<Element>();

                for (int i = 0; i < Targets.Count; i++)
                {
                    var instance = new Element(this, Targets[i]);

                    Elements.Add(instance);

                    Elements[i].Register();
                }
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
        }
    }
}