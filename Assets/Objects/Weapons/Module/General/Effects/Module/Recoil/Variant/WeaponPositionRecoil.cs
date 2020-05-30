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
	public class WeaponPositionRecoil : WeaponRecoil
	{
        protected override void Reset()
        {
            base.Reset();

            kick = new ValueRange(-0.05f, -0.075f);
            sway = new SwayData(0.01f);
            speed = new SpeedData(10, 20);
        }

        protected override void Apply(Vector3 value) => Context.localPosition += value;

        protected override Vector3 CalculateTarget()
        {
            return new Vector3()
            {
                x = Random.Range(-sway.Horizontal, sway.Horizontal),
                y = Random.Range(-sway.Vertical, sway.Vertical),
                z = kick.Random
            };
        }
    }
}