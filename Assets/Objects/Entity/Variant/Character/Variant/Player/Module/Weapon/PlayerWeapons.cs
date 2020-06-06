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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class PlayerWeapons : Player.Module, CharacterWeapons.IInterface
	{
        public List<Weapon> List { get; protected set; }

        public int Index { get; protected set; }

        public Weapon Current => List[Index];

        public PlayerWeaponProcessor Processor { get; protected set; }
        Weapon.IProcessor CharacterWeapons.IInterface.Processor => Processor;

        public PlayerWeaponsCamera camera { get; protected set; }

        public class Module : Player.BaseModule<PlayerWeapons>
        {
            public PlayerWeapons Weapons => Reference;

            public override Player Player => Reference.Player;
        }

        public Modules.Collection<PlayerWeapons> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            List = Dependancy.GetAll<Weapon>(gameObject);

            Modules = new Modules.Collection<PlayerWeapons>(this);

            Processor = Modules.Depend<PlayerWeaponProcessor>();

            camera = Modules.Depend<PlayerWeaponsCamera>();

            Character.Weapons.Set(this);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;

            for (int i = 0; i < List.Count; i++)
                List[i].Setup(Character.Weapons);

            Index = List.FindIndex((Weapon instance) => instance.gameObject.activeSelf);

            if (Index < 0) Index = 0;

            List[Index].Equip();

            Modules.Init();
        }

        protected virtual void Equip(int target)
        {
            List[Index].UnEquip();

            Index = target;

            List[Index].Equip();
        }

        void Process()
        {
            if (Input.GetKeyDown(KeyCode.X)) Switch(Index + 1);

            Current.Process();
        }

        protected virtual void Switch(int target)
        {
            if (target < 0) target = List.Count - 1;
            if (target >= List.Count) target = 0;

            Equip(target);
        }
    }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}