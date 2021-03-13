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

        public Modules<WeaponProjectileAction> Modules { get; protected set; }
        public class Module : Weapon.Behaviour, IModule<WeaponProjectileAction>
        {
            public WeaponProjectileAction Action { get; protected set; }
            public virtual void Set(WeaponProjectileAction value) => Action = value;

            public Weapon Weapon => Action.Weapon;
        }

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponProjectileAction>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Action.OnPerform += Action;

            if (point == null) point = transform;
        }

        void Action()
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