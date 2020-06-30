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
    public class AI : MonoBehaviour, IBehaviour<Character>, IModule<Character>
    {
        public Rigidbody rigidbody => Character.rigidbody;

        public AIController Controller { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<AI>
        {

        }
        public Behaviours.Collection<AI> Behaviours { get; protected set; }

        public class Module : Behaviour, IModule<AI>
        {
            public AI AI { get; protected set; }
            public virtual void Setup(AI reference)
            {
                AI = reference;
            }
            
            public Character Character => AI.Character;
            public Entity Entity => Character.Entity;

            public Rigidbody rigidbody => Character.rigidbody;

            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
        }
        public Modules.Collection<AI> Modules { get; protected set; }

        public Character Character { get; protected set; }
        public virtual void Setup(Character reference)
        {
            Character = reference;
        }

        public virtual void Configure()
        {
            Controller = Dependancy.Get<AIController>(gameObject);

            Behaviours = new Behaviours.Collection<AI>(this);
            Behaviours.Register(gameObject);

            Modules = new Modules.Collection<AI>(this);
            Modules.Register(Behaviours);

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