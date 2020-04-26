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
	public class Entity : MonoBehaviour, Entity.IDamageMeta,
        Damage.IDamagable, Damage.IDamager
    {
        #region Health
        public EntityHealth Health { get; protected set; }

        public bool HasHealth => Health != null;

        public bool IsDead => Health.Value == 0f;
        public bool IsAlive => Health.Value > 0f;
        #endregion

        public EntityDamage Damage { get; protected set; }

        public class Module : Module<Entity>
        {
            public Entity Entity => Reference;
        }

        Entity IDamageMeta.Reference => this;

        protected virtual void Awake()
        {
            Health = GetComponentInChildren<EntityHealth>();

            Damage = GetComponentInChildren<EntityDamage>();

            Modules.Configure(this);
        }
        
        protected virtual void Start()
        {
            Modules.Init(this);
        }

        public delegate void DeathDelegate(Damage.IDamager cause);
        public event DeathDelegate OnDeath;
        public virtual void Death(Damage.IDamager cause)
        {
            OnDeath?.Invoke(cause);
        }

        Damage.Meta Damage.IInterface.Meta => Damage.Meta;
        Damage.Result Damage.IDamagable.TakeDamage(Damage.IDamager source, Damage.Request request) => Damage.Take(source, request);
        Damage.Result Damage.IDamager.DoDamage(Damage.IDamagable target, Damage.Request request) => Damage.Do(target, request);

        public interface IDamageMeta : Damage.Meta.IInterface
        {
            Entity Reference { get; }
        }
    }
}