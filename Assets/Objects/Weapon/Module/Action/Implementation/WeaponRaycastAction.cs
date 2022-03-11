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

using MB;

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

        [field: SerializeField, DebugOnly]
        public WeaponPenetration Penetration { get; protected set; }

        RaycastHit[] hits;

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Penetration = Weapon.Modules.Find<WeaponPenetration>();
        }

        public override void Configure()
        {
            base.Configure();

            //Penetration
            {
                var iterations = Penetration == null ? 20 : Penetration.Iterations;
                hits = new RaycastHit[iterations];
            }
        }
        public override void Initialize()
        {
            base.Initialize();

            Weapon.Action.OnPerform += Action;
        }

        void Action()
        {
            if (enabled == false)
                return;

            Shoot();
        }

        protected virtual void Shoot()
        {
            var count = Physics.RaycastNonAlloc(point.position, point.forward, hits, range, Mask);

            Array.Sort(hits, 0, count, HitComparer);

            var power = Penetration.Power;

            for (int i = 0; i < count; i++)
            {
                var rate = power / Penetration.Power;
                if (rate <= 0f) break;

                var surface = Surface.Get(hits[i].collider);

                var data = WeaponHit.Data.From(ref hits[i], point.forward, surface, rate);
                Weapon.Hit.Process(data);

                if (surface == null) break;

                power = Penetration.Evaluate(power, surface);
            }
        }

        //Static Utility

        public static GenericComparer<RaycastHit> HitComparer { get; }

        static WeaponRaycastAction()
        {
            HitComparer = new GenericComparer<RaycastHit>(Compare);
            static int Compare(RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance);
        }
    }
}