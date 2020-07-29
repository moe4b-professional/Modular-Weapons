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
    [RequireComponent(typeof(Collider))]
    public class Character : MonoBehaviour, IBehaviour<Entity>, IModule<Entity>
    {
        public Rigidbody rigidbody { get; protected set; }
        public Collider collider { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<Character>
        {

        }
        public Behaviours.Collection<Character> Behaviours { get; protected set; }

        public class Module : Behaviour, IModule<Character>
        {
            public Character Character { get; protected set; }
            public virtual void Setup(Character reference)
            {
                Character = reference;
            }

            public Entity Entity => Character.Entity;

            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
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

            Behaviours = new Behaviours.Collection<Character>(this);
            Behaviours.Register(gameObject);

            Modules = new Modules.Collection<Character>(this);
            Modules.Register(Behaviours);

            Modules.Configure();
        }

        public virtual void Init()
        {
            Modules.Init();
        }
    }
#pragma warning restore CS0108
}