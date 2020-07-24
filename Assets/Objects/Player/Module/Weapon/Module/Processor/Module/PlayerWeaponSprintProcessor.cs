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
	public class PlayerWeaponSprintProcessor : PlayerWeapons.Processor, WeaponSprint.IProcessor
    {
        public ControllerSprint Sprint => Player.Controller.Sprint;

        public float Weight => Sprint.Weight;

        public bool Active => Sprint.Active;

        public float Target => Sprint.Target;
    }
}