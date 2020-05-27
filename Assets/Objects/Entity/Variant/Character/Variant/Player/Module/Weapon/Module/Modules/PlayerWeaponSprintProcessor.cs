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
	public class PlayerWeaponSprintProcessor : PlayerWeaponProcessor.Module, WeaponSprint.IProcess
    {
        public float Weight => Player.Controller.State.Sets.Sprint.Weight;

        public float Axis => Player.Controller.State.Sets.Sprint.Axis;
    }
}