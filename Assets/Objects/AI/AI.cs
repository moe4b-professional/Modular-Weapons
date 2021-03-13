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
    public class AI : Character.Module
    {
        public Rigidbody rigidbody => Character.rigidbody;

        public AIController Controller { get; protected set; }

        #region Behaviours
        public Behaviours<AI> Behaviours { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<AI>
        {
            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
        }
        #endregion

        #region Modules
        public Modules<AI> Modules { get; protected set; }
        public class Module : Behaviour, IModule<AI>
        {
            public AI AI { get; protected set; }
            public virtual void Set(AI reference)
            {
                AI = reference;
            }
            
            public Character Character => AI.Character;
            public Entity Entity => Character.Entity;
        }
        #endregion

        public override void Configure()
        {
            base.Configure();

            Behaviours = new Behaviours<AI>(this);

            Modules = new Modules<AI>(this);
            Modules.Register(Behaviours);

            Controller = Modules.Depend<AIController>();

            Modules.Set();

            Behaviours.Configure();
        }

        public override void Init()
        {
            base.Init();

            Behaviours.Init();
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