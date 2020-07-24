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
    public class PlayerWeaponStepProcessor : PlayerWeapons.Processor, WeaponStep.IProcessor
    {
        public ControllerStep Step => Player.Controller.Step;

        public float Rate => Step.Rate;

        public float Time => Step.Time;

        public int Count => Step.Count;

        public float Weight => Step.Weight.Value;
    }
}