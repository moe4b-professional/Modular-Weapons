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
        public Modifier.Scale Scale { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;
        public Transform Context => Pivot.transform;

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            Vector3 Delta { get; }
        }

        public class Module : Weapon.BaseModule<WeaponBob>
        {
            public WeaponBob Bob => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }
        public Modules.Collection<WeaponBob> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Scale = new Modifier.Scale();

            Modules = new Modules.Collection<WeaponBob>(this);

            Modules.Register(Weapon.Behaviours);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Pivot.OnProcess += Process;

            Modules.Init();
        }

        public event Action OnProcess;
        void Process()
        {
            OnProcess?.Invoke();
        }
	}
}