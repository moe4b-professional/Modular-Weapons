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
	public class WeaponProjectileParentAction : WeaponProjectileAction.Module
    {
        [SerializeField]
        protected Transform parent;
        public Transform Parent { get { return parent; } }

        protected virtual void Reset()
        {
            parent = transform;
        }

        public override void Initialize()
        {
            base.Initialize();

            Action.OnPerform += Callback;
        }

        void Callback(Projectile projectile)
        {
            projectile.transform.SetParent(parent);
        }
    }
}