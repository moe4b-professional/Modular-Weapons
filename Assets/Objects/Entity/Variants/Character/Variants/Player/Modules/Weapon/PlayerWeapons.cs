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
        [SerializeField]
        protected Weapon weapon;
        public Weapon Weapon { get { return weapon; } }

        public PlayerWeaponsProcess Process { get; protected set; }

        public class Module : Player.Module
        {
            public PlayerWeapons Weapons => Player.Weapons;
        }

        public override void Configure(Player reference)
        {
            base.Configure(reference);

            Process = GetComponentInChildren<PlayerWeaponsProcess>();
        }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += ProcessCallback;

            weapon.Setup(Player.Character);
        }

        void ProcessCallback()
        {
            weapon.Process(Process);
        }
    }
}