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
	public class PlayerWeaponSprintProcessor : PlayerWeaponProcessor.Module, WeaponSprint.IProcessor
    {
        public ControllerSprint Sprint => Player.Controller.Sprint;

        public bool Active => Sprint.Active;

        public float Weight => Sprint.Weight;
    }
}