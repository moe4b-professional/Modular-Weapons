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
	public class WeaponPenetration : Weapon.Module
	{
        [SerializeField]
        float power = 20f;
        public float Power => power;

        [SerializeField]
        int iterations = 20;
        public int Iterations => iterations;

        public float Evaluate(float value, Surface surface)
        {
            value -= surface.Hardness;

            return value;
        }
    }
}