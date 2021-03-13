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
    public class WeaponRecoil : Weapon.Module, WeaponEffects.IInterface
    {
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        public Transform Context => anchor.transform;

        public Modifier.Scale Scale { get; protected set; }

        public class Module : Weapon.Behaviour, IModule<WeaponRecoil>
        {
            public WeaponRecoil Recoil { get; protected set; }
            public virtual void Set(WeaponRecoil value) => Recoil = value;

            public Weapon Weapon => Recoil.Weapon;
        }

        public Modules<WeaponRecoil> Modules { get; protected set; }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponRecoil>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Weapon.Action.OnPerform += Action;
        }

        public event Action OnAction;
        void Action()
        {
            if (enabled) OnAction?.Invoke();
        }
    }
}