﻿using System;
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
	public class WeaponRotationRecoil : WeaponRecoil
    {
        protected override void Reset()
        {
            base.Reset();

            kick = new ValueRange(-5, 2);
            sway = new SwayData(5);
            speed = new SpeedData(5, 10);
        }

        protected override void Apply(Vector3 value) => Context.localEulerAngles += value;

        protected override Vector3 CalculateTarget()
        {
            return new Vector3()
            {
                x = noise.Lerp(1, kick),
                y = noise.Lerp(3, sway.Vertical),
                z = noise.Lerp(5, sway.Horizontal),
            };
        }
    }
}