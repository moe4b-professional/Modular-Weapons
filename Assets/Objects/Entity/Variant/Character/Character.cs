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
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(Rigidbody))]
	public class Character : MonoBehaviour, IModule<Entity>
    {
        public Rigidbody rigidbody { get; protected set; }

        public Collider collider { get; protected set; }

        public CharacterWeapons Weapons { get; protected set; }

        public class Module : ReferenceModule<Character>
        {
            public Character Character => Reference;

            public Entity Entity => Character.Entity;
        }

        public Modules.Collection<Character> Modules { get; protected set; }

        public Entity Entity { get; protected set; }
        public virtual void Setup(Entity reference)
        {
            Entity = reference;
        }

        public virtual void Configure()
        {
            rigidbody = GetComponent<Rigidbody>();

            collider = GetComponent<Collider>();

            Modules = new Modules.Collection<Character>(this);

            Weapons = Modules.Find<CharacterWeapons>();

            Modules.Configure();
        }

        public virtual void Init()
        {
            Modules.Init();
        }
    }
#pragma warning restore CS0108
}