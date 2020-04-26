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
	public class ZombieAI : AIController
	{
        public override void Init()
        {
            base.Init();

            var target = FindObjectOfType<Player>().Character.Entity;

            var request = new Damage.Request(20, Damage.Method.Contact);

            Entity.DoDamage(target, request);
        }
    }
}