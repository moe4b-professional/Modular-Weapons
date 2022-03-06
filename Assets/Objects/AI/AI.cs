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
    [RequireComponent(typeof(Character))]
    public class AI : Character.Module
    {
        [field: SerializeField, DebugOnly]
        public AIController Controller { get; protected set; }

        #region Modules
        [field: SerializeField, DebugOnly]
        public Behaviours<AI> Behaviours { get; protected set; }
        public class Behaviour : MonoBehaviour, IBehaviour<AI>
        {
            public virtual void Configure()
            {

            }
            public virtual void Initialize()
            {

            }
        }

        [field: SerializeField, DebugOnly]
        public Modules<AI> Modules { get; protected set; }
        public class Module : Behaviour, IModule<AI>
        {
            [field: SerializeField, DebugOnly]
            public AI AI { get; protected set; }

            public Character Character => AI.Character;
            public Entity Entity => Character.Entity;

            public virtual void Set(AI reference)
            {
                AI = reference;
            }
        }
        #endregion

        public Rigidbody rigidbody => Character.rigidbody;

        public override void Set(Character reference)
        {
            base.Set(reference);

            Behaviours = new Behaviours<AI>(this);

            Modules = new Modules<AI>(this);
            Modules.Register(Behaviours);

            Controller = Modules.Depend<AIController>();

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