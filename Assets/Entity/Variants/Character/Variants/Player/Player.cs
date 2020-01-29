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
#pragma warning disable CS0108
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class Player : MonoBehaviour, IModule<Character>, Player.IDamager
    {
        public CapsuleCollider collider { get; protected set; }

        public Rigidbody rigidbody => Character.rigidbody;

        public PlayerWeapons Weapons { get; protected set; }

		public class Module : Module<Player>
        {
            public Player Player => Reference;
            public Character Character => Player.Character;
            public Entity Entity => Character.Entity;
        }

        Player IDamager.Player => this;

        public Character Character { get; protected set; }
        public virtual void Configure(Character reference)
        {
            Character = reference;

            collider = Character.collider as CapsuleCollider;

            Modules.Configure(this);

            Weapons = GetComponentInChildren<PlayerWeapons>();
        }

        public Entity Entity => Character.Entity;

        public virtual void Init()
        {
            Modules.Init(this);
        }

        protected virtual void Update()
        {
            Process();
        }

        public event Action OnProcess;
        protected virtual void Process()
        {
            OnProcess?.Invoke();
        }

        public interface IDamager : Character.IDamager
        {
            Player Player { get; }
        }
    }
#pragma warning restore CS0108
}