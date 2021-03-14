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
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class Projectile : MonoBehaviour
	{
        public Rigidbody rigidbody { get; protected set; }

        public Collider collider { get; protected set; }

        public Bounds Bounds => collider.bounds;

        public float Radius => (Bounds.size.x + Bounds.size.y + Bounds.size.z) / 3f;

        public Behaviours<Projectile> Behaviours { get; protected set; }
        public class Behaviour : MonoBehaviour, IBehaviour<Projectile>
        {
            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }
        }

        public Modules<Projectile> Modules { get; protected set; }
        public class Module : Behaviour, IModule<Projectile>
        {
            public Projectile Projectile { get; protected set; }
            public virtual void Set(Projectile reference)
            {
                Projectile = reference;
            }
        }

        public virtual void Setup()
        {
            Configure();

            Init();
        }

        protected virtual void Configure()
        {
            rigidbody = GetComponent<Rigidbody>();

            collider = GetComponent<Collider>();

            Behaviours = new Behaviours<Projectile>(this);

            Modules = new Modules<Projectile>(this);
            Modules.Register(Behaviours);

            Modules.Set();

            Behaviours.Configure();
        }

        protected virtual void Init()
        {
            Behaviours.Init();
        }

        public virtual void IgnoreCollisions(GameObject target)
        {
            var targets = Dependancy.GetAll<Collider>(target);

            for (int i = 0; i < targets.Count; i++)
                IgnoreCollisions(targets[i]);
        }
        public virtual void IgnoreCollisions(Collider target)
        {
            Physics.IgnoreCollision(collider, target, true);
        }

        #region Hit
        void OnCollisionEnter(Collision collision)
        {
            var data = new WeaponHit.Data(collision.collider, collision.contacts[0], rigidbody.velocity.normalized);

            ProcessHit(data);
        }

        void OnTriggerEnter(Collider collider)
        {
            var contact = new WeaponHit.Contact(transform.forward * (Radius / 2f), -transform.forward);

            var data = new WeaponHit.Data(collider, contact, rigidbody.velocity.normalized);

            ProcessHit(data);
        }

        public delegate void HitDelegate(Projectile projectile, WeaponHit.Data data);
        public event HitDelegate OnHit;
        protected virtual void ProcessHit(WeaponHit.Data data)
        {
            OnHit?.Invoke(this, data);
        }
        #endregion

        public delegate void DestroyCallback(Projectile projectile);
        public event DestroyCallback OnDestroy;
        public virtual void Destroy()
        {
            OnDestroy?.Invoke(this);

            Destroy(gameObject);
        }
    }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}