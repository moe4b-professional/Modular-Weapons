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
	public class PlayerWeapons : Player.Module
	{
        public List<Weapon> List { get; protected set; }

        public int Index { get; protected set; }

        public Weapon Current => List[Index];

        public PlayerWeaponsProcess Process { get; protected set; }

        public class Module : ReferenceBehaviour<PlayerWeapons>
        {
            public PlayerWeapons Weapons => Reference;

            public Player Player => Weapons.Player;
        }

        public override void Configure(Player reference)
        {
            base.Configure(reference);

            List = this.GetAllDependancies<Weapon>();

            Process = this.GetDependancy<PlayerWeaponsProcess>();

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += ProcessCallback;

            for (int i = 0; i < List.Count; i++)
            {
                List[i].Setup(Player.Character);
            }

            Index = List.FindIndex((Weapon instance) => instance.gameObject.activeSelf);

            if (Index < 0) Index = 0;

            List[Index].Equip();

            References.Init(this);
        }

        protected virtual void Equip(int target)
        {
            List[Index].UnEquip();

            Index = target;

            List[Index].Equip();
        }

        void ProcessCallback()
        {
            if (Input.GetKeyDown(KeyCode.X)) Switch(Index + 1);

            Current.Process(Process);
        }

        protected virtual void Switch(int target)
        {
            if (target < 0) target = List.Count - 1;
            if (target >= List.Count) target = 0;

            Equip(target);
        }
    }
}