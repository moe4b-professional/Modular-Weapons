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
	public class ProjectileGravity : Projectile.Module
	{
        [SerializeField]
        Vector3 force = new Vector3(0f, 1f, 0f);
        public Vector3 Force => force;

        public override void Init()
        {
            base.Init();

            Projectile.OnProcess += Process;
        }

        void Process()
        {
            Projectile.transform.position -= force * Time.deltaTime;
        }
    }
}