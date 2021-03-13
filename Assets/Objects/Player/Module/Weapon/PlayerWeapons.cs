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
    public class PlayerWeapons : Player.Module, Weapon.IOwner
    {
        public List<Weapon> List { get; protected set; }
        public int Index { get; protected set; }
        public Weapon Current => List[Index];

        public GameObject Root => Player.gameObject;

        public Damage.IDamager Damager => Entity;

        public PlayerWeaponsCamera camera { get; protected set; }

        public Modules<PlayerWeapons> Modules { get; protected set; }
        public class Module : Player.Behaviour, IModule<PlayerWeapons>
        {
            public PlayerWeapons Weapons { get; protected set; }
            public virtual void Set(PlayerWeapons value) => Weapons = value;

            public Player Player => Weapons.Player;
        }

        public class Processor : Module
        {

        }
        public TType GetProcessor<TType>() where TType : class => Modules.Find<TType>();

        public PlayerControls Controls => Player.Controls;

        public override void Set(Player value)
        {
            base.Set(value);

            Modules = new Modules<PlayerWeapons>(this);
            Modules.Register(Player.Behaviours);

            camera = Modules.Depend<PlayerWeaponsCamera>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            List = Dependancy.GetAll<Weapon>(gameObject);
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;

            for (int i = 0; i < List.Count; i++)
                List[i].Setup(this);

            Index = List.FindIndex((Weapon instance) => instance.gameObject.activeSelf);

            if (Index < 0) Index = 0;

            List[Index].Equip();
        }

        protected virtual void Equip(int target)
        {
            List[Index].UnEquip();

            Index = target;

            List[Index].Equip();
        }

        void Process()
        {
            if(Controls.SwitchWeapon.Positive.Press) Switch(Index + 1);
            if(Controls.SwitchWeapon.Negative.Press) Switch(Index - 1);

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