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
	public class PlayerWeaponFOVProcessor : PlayerWeapons.Processor, WeaponFOV.IProcessor
    {
        public float Scale { get; set; } = 1f;

        public float Modifier() => Scale;

        public override void Initialize()
        {
            base.Initialize();

            Player.Controller.camera.FOV.Scale.Add(Modifier);
        }
    }
}