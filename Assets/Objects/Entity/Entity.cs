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

        public class Module : Module<Entity>
        {
            public Entity Entity => Reference;
        }

        Entity IDamageMeta.Entity => this;

        public Damage.IDamager Damager => this;

        protected virtual void Awake()
        {
            Health = GetComponentInChildren<EntityHealth>();

            Modules.Configure(this);
        }
        
        protected virtual void Start()
        {
            Modules.Init(this);
        }

        #region Damage
        public delegate void TakeDamgeDelegate(Damage.Result result);
        public event TakeDamgeDelegate OnTakeDamage;
        public virtual Damage.Result TakeDamage(Damage.IDamager source, Damage.Request request)
        {
            if (IsDead)
            {
                //TODO
            }
            else
            {
                Health.Value -= request.Value;

                if (Health.Value == 0f)
                    Death(source);
            }

            var result = new Damage.Result(source, this, request);

            OnTakeDamage?.Invoke(result);

            return result;
        }

        public delegate void DoDamageDelegate(Damage.Result result);
        public event DoDamageDelegate OnDoDamage;
        public virtual Damage.Result DoDamage(Damage.IDamagable target, Damage.Request request)
        {
            var result = Damage.Invoke(Damager, target, request);

            OnDoDamage?.Invoke(result);

            return result;
        }

        public delegate void DeathDelegate(Damage.IDamager cause);
        public event DeathDelegate OnDeath;
        protected virtual void Death(Damage.IDamager cause)
        {
            OnDeath?.Invoke(cause);
        }

        public interface IDamageMeta : Damage.IMeta
        {
            Entity Entity { get; }
        }
        #endregion
    }
}