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

        public class Module : Player.Module<PlayerWeaponProcessor>
        {
            public PlayerWeaponProcessor Processor => Reference;

            public Player Player => Reference.Player;
        }

        public override void Configure(PlayerWeapons reference)
        {
            base.Configure(reference);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            References.Init(this);
        }

        public T GetDependancy<T>() where T : class => Dependancy.Get<T>(gameObject);
    }
}