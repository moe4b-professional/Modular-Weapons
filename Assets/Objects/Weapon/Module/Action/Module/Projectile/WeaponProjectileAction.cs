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
	public class WeaponProjectileAction : Weapon.Module
	{
		[SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        [SerializeField]
        protected Transform point;
        public Transform Point { get { return point; } }

        public class Module : Weapon.BaseModule<WeaponProjectileAction>
        {
            public WeaponProjectileAction Action => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }
        public Modules.Collection<WeaponProjectileAction> Modules { get; protected set; }

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<WeaponProjectileAction>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Action.OnPerform += ActionCallback;

            if (point == null) point = transform;

            Modules.Init();
        }

        void ActionCallback()
        {
            if (enabled) Perform();
        }

        public delegate void PerformDelegate(Projectile projectile);
        public event PerformDelegate OnPerform;
        protected virtual void Perform()
        {
            var projectile = Instantiate();

            projectile.IgnoreCollisions(Weapon.Owner.Root);

            OnPerform?.Invoke(projectile);
        }

        public virtual Projectile Instantiate()
        {
            var instance = Instantiate(prefab, point.position, point.rotation);

            var script = instance.GetComponent<Projectile>();

            script.Setup();

            return script;
        }
    }
}