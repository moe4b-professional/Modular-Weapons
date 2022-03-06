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
    [RequireComponent(typeof(FirstPersonController))]
    public class Player : Character.Module
    {
        #region Modules
        [field: SerializeField, DebugOnly]
        public FirstPersonController Controller { get; protected set; }

        [field: SerializeField, DebugOnly]
        public PlayerControls Controls { get; protected set; }

        [field: SerializeField, DebugOnly]
        public PlayerCameraEffects CameraEffects { get; protected set; }

        [field: SerializeField, DebugOnly]
        public PlayerWeapons Weapons { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Behaviours<Player> Behaviours { get; protected set; }
        public class Behaviour : MonoBehaviour, IBehaviour<Player>
        {
            public virtual void Configure()
            {

            }

            public virtual void Initialize()
            {

            }
        }

        [field: SerializeField, DebugOnly]
        public Modules<Player> Modules { get; protected set; }
        public abstract class Module : Behaviour, IModule<Player>
        {
            [field: SerializeField, DebugOnly]
            public virtual Player Player { get; protected set; }

            public Entity Entity => Player.Entity;
            public Character Character => Player.Character;

            public virtual void Set(Player value) => Player = value;
        }
        #endregion

        public override void Set(Character reference)
        {
            base.Set(reference);

            Controller = GetComponent<FirstPersonController>();

            Behaviours = new Behaviours<Player>(this);

            Modules = new Modules<Player>(this);
            Modules.Register(Behaviours);

            Controls = Modules.Depend<PlayerControls>();
            CameraEffects = Modules.Depend<PlayerCameraEffects>();
            Weapons = Modules.Depend<PlayerWeapons>();

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