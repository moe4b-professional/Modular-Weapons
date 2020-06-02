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
	public class PlayerWeaponBobProcessor : PlayerWeaponProcessor.Module, WeaponBob.IProcessor
    {
        public ControllerHeadBob HeadBob => Player.Controller.HeadBob;

        public Vector3 Delta => HeadBob.Delta;
    }
}