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
    public class PlayerWeaponProcessor : PlayerWeapons.Module, Weapon.IProcessor
    {
        public bool Input => Player.Input.Primary.Held;

        public class Module : Player.BaseModule<PlayerWeaponProcessor>
        {
            public PlayerWeaponProcessor Processor => Reference;

            public override Player Player => Reference.Player;
        }

        public Modules.Collection<PlayerWeaponProcessor> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<PlayerWeaponProcessor>(this);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }

        public T GetDependancy<T>() where T : class => Dependancy.Get<T>(gameObject);
    }
}