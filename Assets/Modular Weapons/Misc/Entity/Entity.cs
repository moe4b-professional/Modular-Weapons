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
	public class Entity : MonoBehaviour, IDamager, IDamagable
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
        public virtual Damage.Result TakeDamage(Damage.Request request)
        {
            if(IsDead)
            {
                //TODO
            }
            else
            {
                Health.Value -= request.Value;

                if (Health.Value == 0f)
                    Death(request.Source);
            }

            var result = new Damage.Result(this, request);

            OnTakeDamage?.Invoke(result);

            return result;
        }

        public delegate void DoDamageDelegate(Damage.Result result);
        public event DoDamageDelegate OnDoDamage;
        public virtual Damage.Result DoDamage(IDamagable target, Damage.Request request)
        {
            var result = Damage.Invoke(target, request);

            OnDoDamage?.Invoke(result);

            return result;
        }
        public virtual Damage.Result? DoDamage(GameObject target, Damage.Request request)
        {
            var damagable = target.GetComponent<IDamagable>();

            if (damagable == null)
                return null;

            return DoDamage(damagable, request);
        }
        public virtual Damage.Result DoDamage(IDamagable target, float value, Damage.Method method)
        {
            var request = new Damage.Request(this, value, method);

            return DoDamage(target, request);
        }
        public virtual Damage.Result? DoDamage(GameObject target, float value, Damage.Method method)
        {
            var damagable = target.GetComponent<IDamagable>();

            if (damagable == null) return null;

            return DoDamage(damagable, value, method);
        }

        public delegate void DeathDelegate(IDamager cause);
        public event DeathDelegate OnDeath;
        protected virtual void Death(IDamager cause)
        {
            OnDeath?.Invoke(cause);
        }
        #endregion
    }
}