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
	public class PlayerWeaponSwayProcessor : PlayerWeaponProcessor.Module, WeaponSway.IProcessor
    {
        public Transform Anchor => Player.Controller.camera.transform;

        public Vector3 LookDelta => Player.Controller.Look.Delta;

        public Vector3 RelativeVelocity => Player.Controller.Velocity.Relative;
    }
}