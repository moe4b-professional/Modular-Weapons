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
        protected float range = Mathf.Infinity;
        public float Range { get { return range; } }

        [SerializeField]
        protected LayerMask mask = Physics.DefaultRaycastLayers;
        public LayerMask Mask { get { return mask; } }

        RaycastHit hit;

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += ActionCallback;
        }

        void ActionCallback()
        {
            if(Physics.Raycast(transform.position, transform.forward, out hit, range, mask))
            {
                Debug.Log("Hit: " + hit.transform.name);
            }
            else
            {

            }
        }
    }
}