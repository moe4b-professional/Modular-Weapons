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

        RaycastHit hit;

        protected virtual void Reset()
        {
            point = transform;
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += ActionCallback;
        }

        void ActionCallback()
        {
            if (enabled) Shoot();
        }

        protected virtual void Shoot()
        {
            if (Physics.Raycast(point.position, point.forward, out hit, range, mask))
            {
                var data = new HitData(hit, point.forward);

                Weapon.Hit.Process(data);
            }
            else
            {

            }
        }
    }
}