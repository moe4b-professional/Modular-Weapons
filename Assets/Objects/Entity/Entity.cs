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
	public class Entity : MonoBehaviour, Damage.IDamagable
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

        public Damage.IDamager Damager { get; protected set; }

        protected virtual void Awake()
        {
            Health = GetComponentInChildren<EntityHealth>();

            Damager = GetComponent<Damage.IDamager>();

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
            if (request.Source is IDamager)
            {
                var source = request.Source as IDamager;

                Debug.Log(name + " Took Damage From Entity: " + source.Entity.name);
            }

            if (request.Source is Player.IDamager)
            {
                var source = request.Source as Player.IDamager;

                Debug.Log(name + " Took Damage From Player: " + source.Player.name);
            }

            if (IsDead)
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
        public virtual Damage.Result DoDamage(Damage.IDamagable target, Damage.Request request)
        {
            var result = Damage.Invoke(target, request);

            OnDoDamage?.Invoke(result);

            return result;
        }
        public virtual Damage.Result DoDamage(Damage.IDamagable target, float value, Damage.Method method)
        {
            var request = new Damage.Request(Damager, value, method);

            return DoDamage(target, request);
        }
        public virtual Damage.Result? DoDamage(GameObject target, float value, Damage.Method method)
        {
            var damagable = target.GetComponent<Damage.IDamagable>();

            if (damagable == null) return null;

            return DoDamage(damagable, value, method);
        }

        public delegate void DeathDelegate(Damage.IDamager cause);
        public event DeathDelegate OnDeath;
        protected virtual void Death(Damage.IDamager cause)
        {
            OnDeath?.Invoke(cause);
        }
        #endregion

        public interface IDamager : Damage.IDamager
        {
            Entity Entity { get; }
        }
    }
}