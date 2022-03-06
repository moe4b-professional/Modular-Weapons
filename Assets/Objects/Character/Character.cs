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

using MB;

namespace Game
{
#pragma warning disable CS0108
    [RequireComponent(typeof(Entity))]
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Character : Entity.Module
    {
        [field: SerializeField, DebugOnly]
        public Rigidbody rigidbody { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Collider collider { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Behaviours<Character> Behaviours { get; protected set; }
        public class Behaviour : MonoBehaviour, IBehaviour<Character>
        {
            public virtual void Configure()
            {

            }
            public virtual void Initialize()
            {

            }
        }

        [field: SerializeField, DebugOnly]
        public Modules<Character> Modules { get; protected set; }
        public class Module : Behaviour, IModule<Character>
        {
            [field: SerializeField, DebugOnly]
            public Character Character { get; protected set; }

            public Entity Entity => Character.Entity;

            public virtual void Set(Character reference)
            {
                Character = reference;
            }
        }

        public override void Set(Entity reference)
        {
            base.Set(reference);

            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();

            Behaviours = new Behaviours<Character>(this);

            Modules = new Modules<Character>(this);
            Modules.Register(Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Behaviours.Configure();
        }
        public override void Initialize()
        {
            base.Initialize();

            Behaviours.Initialize();
        }
    }
#pragma warning restore CS0108
}