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
    public class WeaponRotatingBarrelConstraint : WeaponRotatingBarrel.Module, WeaponConstraint.IInterface
    {
        [SerializeField]
        [Range(0f, 1f)]
        protected float minRate;
        public float MinRate { get { return minRate; } }

        bool WeaponConstraint.IInterface.Constraint => RotatingBarrel.Rate < minRate;
    }
}