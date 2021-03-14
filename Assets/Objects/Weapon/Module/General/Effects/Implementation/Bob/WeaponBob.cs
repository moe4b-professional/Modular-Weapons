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
	public class WeaponBob : Weapon.Module, WeaponEffects.IInterface
    {
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        public Transform Context => anchor.transform;

        public Modifier.Scale Scale { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            Vector3 Delta { get; }
        }

        public Modules<WeaponBob> Modules { get; protected set; }
        public class Module : Weapon.Behaviour, IModule<WeaponBob>
        {
            public WeaponBob Bob { get; protected set; }
            public virtual void Set(WeaponBob value) => Bob = value;

            public Weapon Weapon => Bob.Weapon;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponBob>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);

            Scale = new Modifier.Scale();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);
        }
	}
}