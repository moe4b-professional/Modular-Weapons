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
                x = kick.Random,
                y = Random.Range(-sway.Vertical, sway.Vertical),
                z = Random.Range(-sway.Vertical, sway.Vertical)
            };
        }
    }
}