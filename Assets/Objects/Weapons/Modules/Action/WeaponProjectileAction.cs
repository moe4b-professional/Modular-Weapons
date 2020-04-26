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

        [SerializeField]
        protected float velocity = 5f;
        public float Velocity { get { return velocity; } }

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += Action;

            if (point == null) point = transform;
        }

        void Action()
        {
            var instance = Spawn();

            instance.IgnoreCollisions(Weapon.Owner.gameObject);
            instance.AddVelocity(point.forward, velocity);
            instance.Arm();

            instance.OnHit += ProjectileHitCallback;
        }

        void ProjectileHitCallback(Projectile projectile, Collider collider)
        {
            Debug.Log(projectile.name + " Hit: " + collider.name);

            projectile.Destroy();
        }

        public virtual Projectile Spawn()
        {
            var instance = Instantiate(prefab, point.position, point.rotation);

            var script = instance.GetComponent<Projectile>();

            script.Configure();

            return script;
        }
    }
}