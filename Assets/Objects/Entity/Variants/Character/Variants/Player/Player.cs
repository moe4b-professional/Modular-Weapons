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
    public class Player : MonoBehaviour, IReference<Character>
    {
        public CapsuleCollider collider { get; protected set; }

        public Rigidbody rigidbody => Character.rigidbody;

        public PlayerWeapons Weapons { get; protected set; }
        public PlayerLook Look { get; protected set; }
        public class Module : ReferenceBehaviour<Player>
        {
            public Player Player => Reference;
            public Character Character => Player.Character;
            public Entity Entity => Character.Entity;
        }

        public Character Character { get; protected set; }
        public virtual void Configure(Character reference)
        {
            Character = reference;

            collider = Character.collider as CapsuleCollider;

            Look = this.GetDependancy<PlayerLook>();

            Weapons = this.GetDependancy<PlayerWeapons>();

            References.Configure(this);
        }

        public virtual void Init()
        {
            References.Init(this);
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
    }
#pragma warning restore CS0108
}