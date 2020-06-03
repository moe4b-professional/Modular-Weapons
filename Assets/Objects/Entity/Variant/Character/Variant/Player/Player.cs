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

        public abstract class BaseModule<T> : ReferenceBehaviour<T>
        {
            public abstract Player Player { get; }
            public Character Character => Player.Character;
            public Entity Entity => Character.Entity;
        }
        public abstract class Module : BaseModule<Player>
        {
            public override Player Player => Reference;
        }

        public References.Collection<Player> Modules { get; protected set; }

        public Character Character { get; protected set; }
        public virtual void Set(Character reference)
        {
            Character = reference;
        }

        public virtual void Configure()
        {
            Controller = GetComponent<FirstPersonController>();

            Modules = new References.Collection<Player>(this);

            Input = Modules.Find<PlayerInput>();
            Weapons = Modules.Find< PlayerWeapons>();

            Modules.Configure();
        }

        public virtual void Init()
        {
            Modules.Init();
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