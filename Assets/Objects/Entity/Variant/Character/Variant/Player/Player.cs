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
    [RequireComponent(typeof(FirstPersonController))]
    public class Player : MonoBehaviour, IReference<Character>
    {
        public FirstPersonController Controller { get; protected set; }

        public PlayerInput Input { get; protected set; }
        public PlayerWeapons Weapons { get; protected set; }
        public abstract class Module<T> : ReferenceBehaviour<T>
        {

        }
        public abstract class Module : Module<Player>
        {
            public Player Player => Reference;
            public Character Character => Player.Character;
            public Entity Entity => Character.Entity;
        }

        public Character Character { get; protected set; }
        public virtual void Configure(Character reference)
        {
            Character = reference;

            Controller = GetComponent<FirstPersonController>();

            Input = Dependancy.Get<PlayerInput>(gameObject);

            Weapons = Dependancy.Get< PlayerWeapons>(gameObject);

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