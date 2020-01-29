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
    public class AI : MonoBehaviour, IModule<Character>, AI.IDamager
    {
        public Rigidbody rigidbody => Character.rigidbody;

        public AIController Controller { get; protected set; }

        public class Module : Module<AI>
        {
            public AI AI => Reference;
            public Character Character => AI.Character;
            public Entity Entity => Character.Entity;

            public Rigidbody rigidbody => Character.rigidbody;
        }

        public Character Character { get; protected set; }
        public virtual void Configure(Character reference)
        {
            Character = reference;

            Controller = GetComponentInChildren<AIController>();

            Modules.Configure(this);
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

        public interface IDamager
        {

        }
    }
#pragma warning restore CS0108
}