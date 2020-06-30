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
	public class Entity : MonoBehaviour,
        Damage.IDamagable, Damage.IDamager
    {
        #region Health
        public EntityHealth Health { get; protected set; }

        public bool HasHealth => Health != null;

        public bool IsDead => Health.Value == 0f;
        public bool IsAlive => Health.Value > 0f;
        #endregion

        public EntityDamage Damage { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<Entity>
        {

        }
        public Behaviours.Collection<Entity> Behaviours { get; protected set; }

        public class Module : Behaviour, IModule<Entity>
        {
            public Entity Entity { get; protected set; }
            public virtual void Setup(Entity reference)
            {
                Entity = reference;
            }

            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
        }
        public Modules.Collection<Entity> Modules { get; protected set; }

        protected virtual void Awake()
        {
            Behaviours = new Behaviours.Collection<Entity>(this);
            Behaviours.Register(gameObject);

            Modules = new Modules.Collection<Entity>(this);
            Modules.Register(Behaviours);

            Health = Modules.Depend<EntityHealth>();
            Damage = Modules.Depend<EntityDamage>();

            Modules.Configure();
        }
        
        protected virtual void Start()
        {
            Modules.Init();
        }

        public delegate void DeathDelegate(Damage.IDamager cause);
        public event DeathDelegate OnDeath;
        public virtual void Death(Damage.IDamager cause)
        {
            OnDeath?.Invoke(cause);
        }

        Damage.Result Damage.IDamagable.Take(Damage.IDamager source, Damage.Request request) => Damage.Take(source, request);
        Damage.Result Damage.IDamager.Perform(Damage.IDamagable target, Damage.Request request) => Damage.Perform(target, request);
    }
}