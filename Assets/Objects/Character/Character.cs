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
    public class Character : Entity.Module
    {
        public Rigidbody rigidbody { get; protected set; }
        public Collider collider { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<Character>
        {
            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
        }
        public Behaviours<Character> Behaviours { get; protected set; }

        public class Module : Behaviour, IModule<Character>
        {
            public Character Character { get; protected set; }
            public virtual void Set(Character reference)
            {
                Character = reference;
            }

            public Entity Entity => Character.Entity;
        }
        public Modules<Character> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            rigidbody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();

            Behaviours = new Behaviours<Character>(this);

            Modules = new Modules<Character>(this);
            Modules.Register(Behaviours);

            Modules.Set();

            Behaviours.Configure();
        }

        public override void Init()
        {
            base.Init();

            Behaviours.Init();
        }
    }
#pragma warning restore CS0108
}