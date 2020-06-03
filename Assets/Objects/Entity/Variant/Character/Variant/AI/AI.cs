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
    public class AI : MonoBehaviour, IReference<Character>
    {
        public Rigidbody rigidbody => Character.rigidbody;

        public AIController Controller { get; protected set; }

        public class Module : ReferenceBehaviour<AI>
        {
            public AI AI => Reference;
            public Character Character => AI.Character;
            public Entity Entity => Character.Entity;

            public Rigidbody rigidbody => Character.rigidbody;
        }

        public References.Collection<AI> Modules { get; protected set; }

        public Character Character { get; protected set; }
        public virtual void Set(Character reference)
        {
            Character = reference;
        }

        public virtual void Configure()
        {
            Controller = Dependancy.Get<AIController>(gameObject);

            Modules = new References.Collection<AI>(this);

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