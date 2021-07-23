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
        public FirstPersonController Controller { get; protected set; }

        public PlayerControls Controls { get; protected set; }
        public PlayerCameraEffects CameraEffects { get; protected set; }
        public PlayerWeapons Weapons { get; protected set; }

        #region Behaviours
        public Behaviours<Player> Behaviours { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<Player>
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
        public Modules<Player> Modules { get; protected set; }
        public abstract class Module : Behaviour, IModule<Player>
        {
            public virtual Player Player { get; protected set; }
            public virtual void Set(Player value) => Player = value;

            public Entity Entity => Player.Entity;
            public Character Character => Player.Character;
        }
        #endregion

        public override void Configure()
        {
            base.Configure();

            Controller = GetComponent<FirstPersonController>();

            Behaviours = new Behaviours<Player>(this);

            Modules = new Modules<Player>(this);
            Modules.Register(Behaviours);

            Controls = Modules.Depend<PlayerControls>();
            CameraEffects = Modules.Depend<PlayerCameraEffects>();
            Weapons = Modules.Depend<PlayerWeapons>();

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