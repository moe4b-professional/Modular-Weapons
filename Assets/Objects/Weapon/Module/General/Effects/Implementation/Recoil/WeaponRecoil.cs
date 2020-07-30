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

        public class Module : Weapon.BaseModule<WeaponRecoil>
        {
            public WeaponRecoil Recoil => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }

        public Modules.Collection<WeaponRecoil> Modules { get; protected set; }
        
        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();

            Modules = new Modules.Collection<WeaponRecoil>(this);

            Modules.Register(Weapon.Behaviours);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Weapon.Action.OnPerform += Action;

            Modules.Init();
        }

        public event Action OnAction;
        void Action()
        {
            if (enabled) OnAction?.Invoke();
        }
    }
}