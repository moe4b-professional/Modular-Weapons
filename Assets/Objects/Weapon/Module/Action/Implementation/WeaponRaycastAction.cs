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
    public class WeaponRaycastAction : Weapon.Module
    {
        [SerializeField]
        protected float range = 400;
        public float Range { get { return range; } }

        [SerializeField]
        protected Transform point;
        public Transform Point { get { return point; } }

        [SerializeField]
        protected LayerMask mask = Physics.DefaultRaycastLayers;
        public LayerMask Mask { get { return mask; } }

        public WeaponPenetration Penetration { get; protected set; }

        RaycastHit[] hits;

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Configure()
        {
            base.Configure();

            Penetration = Weapon.Modules.Find<WeaponPenetration>();

            hits = new RaycastHit[Penetration == null ? 20 : Penetration.Iterations];
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Action.OnPerform += Action;
        }

        void Action()
        {
            if (enabled) Shoot();
        }

        protected virtual void Shoot()
        {
            var count = Physics.RaycastNonAlloc(point.position, point.forward, hits, range, Mask);

            Array.Sort(hits, 0, count, HitComparison.Instance);

            var power = Penetration.Power;

            for (int i = 0; i < count; i++)
            {
                var surface = Surface.Get(hits[i].collider);

                var rate = power / Penetration.Power;

                var data = WeaponHit.Data.From(ref hits[i], point.forward, rate);

                Weapon.Hit.Process(data);

                if (surface == null) break;

                power = Penetration.Evaluate(power, surface);
            }
        }

        public class HitComparison : IComparer<RaycastHit>
        {
            public int Compare(RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance);

            public static HitComparison Instance { get; protected set; }

            static HitComparison()
            {
                Instance = new HitComparison();
            }
        }
    }
}