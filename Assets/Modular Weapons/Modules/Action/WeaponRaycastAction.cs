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
        protected LayerMask mask = Physics.DefaultRaycastLayers;
        public LayerMask Mask { get { return mask; } }

        RaycastHit hit;

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += Action;
        }

        void Action()
        {
            Debug.DrawLine(transform.position, transform.position + transform.forward * range, Color.red, 5f);

            if (Physics.Raycast(transform.position, transform.forward, out hit, range, mask))
            {
                Debug.Log("Hit: " + hit.transform.name);

                var result = Owner.DoDamage(hit.transform.gameObject, 20, Damage.Method.Undefined);
            }
            else
            {

            }
        }
    }
}