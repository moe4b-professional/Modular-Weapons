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
	public class ProjectileRaycastScan : Projectile.Module
	{
        [SerializeField]
        LayerMask mask;
        public LayerMask Mask => mask;

        Vector3 origin;
        Vector3 anchor;
        HashSet<Collider> hash;

        public override void Configure()
        {
            base.Configure();

            hash = new HashSet<Collider>();
        }
        public override void Initialize()
        {
            base.Initialize();

            origin = Projectile.transform.position;
            anchor = Projectile.transform.position;

            Projectile.OnProcess += Process;
        }

        void Process()
        {
            var delta = Projectile.transform.position - anchor;
            var direction = delta.normalized;
            var range = delta.magnitude;

            var hits = Physics.RaycastAll(anchor, direction, range, mask);

            for (int i = 0; i < hits.Length; i++)
            {
                if (hash.Contains(hits[i].collider)) continue;

                hash.Add(hits[i].collider);

                var distance = Vector3.Distance(origin, hits[i].point);
                var data = WeaponHit.Data.From(ref hits[i], Projectile.Motor.Velocity.normalized, null, 1f, distance);

                Projectile.ProcessHit(data);
            }

            anchor = Projectile.transform.position;
        }
    }
}