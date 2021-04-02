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
	public class ProjectileTranslateMotor : ProjectileMotor
	{
        Vector3 velocity;
        public override Vector3 Velocity
        {
            get => velocity;
            set => velocity = value;
        }

        public override void Init()
        {
            base.Init();

            Projectile.OnProcess += Process;
        }

        void Process()
        {
            Projectile.transform.position += velocity * Time.deltaTime;
        }
    }
}