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
    [RequireComponent(typeof(Camera))]
	public class PlayerWeaponsCamera : PlayerWeapons.Module
	{
		public Camera Component { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Component = GetComponent<Camera>();
        }
    }
}