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
        public Weapon[] Weapons { get; protected set; }

        public int Index { get; protected set; }

        public Weapon Current => Weapons[Index];

        public PlayerWeaponsProcess Process { get; protected set; }

        public class Module : Player.Module
        {
            public PlayerWeapons Weapons => Player.Weapons;
        }

        public override void Configure(Player reference)
        {
            base.Configure(reference);

            Weapons = GetComponentsInChildren<Weapon>(true);

            Process = GetComponentInChildren<PlayerWeaponsProcess>();
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += ProcessCallback;

            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i].Setup(Player.Character);
            }

            Index = Array.FindIndex(Weapons, (Weapon instance) => instance.gameObject.activeSelf);

            if (Index < 0) Index = 0;

            Weapons[Index].Equip();
        }

        protected virtual void Equip(int target)
        {
            Weapons[Index].UnEquip();

            Index = target;

            Weapons[Index].Equip();
        }

        void ProcessCallback()
        {
            if (Input.GetKeyDown(KeyCode.Q)) Switch(Index - 1);
            else if (Input.GetKeyDown(KeyCode.E)) Switch(Index + 1);

            Current.Process(Process);
        }

        protected virtual void Switch(int target)
        {
            if (target < 0) target = Weapons.Length - 1;
            if (target >= Weapons.Length) target = 0;

            Equip(target);
        }
    }
}