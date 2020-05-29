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
        public ControllerSprintStateElement State => Player.Controller.State.Sets.Sprint;

        public bool Active => State.Active;

        public float Weight => State.Weight;

        public float Axis => State.Axis;
    }
}