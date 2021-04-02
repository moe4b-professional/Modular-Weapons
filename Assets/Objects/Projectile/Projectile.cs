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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class Projectile : MonoBehaviour
	{
        public Collider collider { get; protected set; }

        public ProjectileMotor Motor { get; protected set; }

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
            collider = GetComponent<Collider>();

            Behaviours = new Behaviours<Projectile>(this);

            Modules = new Modules<Projectile>(this);
            Modules.Register(Behaviours);

            Motor = Modules.Find<ProjectileMotor>();

            Modules.Set();
            Behaviours.Configure();
        }

        protected virtual void Init()
        {
            Behaviours.Init();
        }

        void Update()
        {
            Process();
        }

        public event Action OnProcess;
        protected virtual void Process()
        {
            OnProcess?.Invoke();
        }

        public delegate void HitDelegate(Projectile projectile, WeaponHit.Data data);
        public event HitDelegate OnHit;
        internal virtual void ProcessHit(WeaponHit.Data data)
        {
            OnHit?.Invoke(this, data);
        }

        public virtual void IgnoreCollisions(GameObject target)
        {
            var targets = Dependancy.GetAll<Collider>(target);

            for (int i = 0; i < targets.Count; i++)
                IgnoreCollisions(targets[i]);
        }
        public virtual void IgnoreCollisions(Collider target)
        {
            if(collider == null)
            {
                Debug.LogWarning($"No Collider Attached to {this}");
                return;
            }

            Physics.IgnoreCollision(collider, target, true);
        }

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