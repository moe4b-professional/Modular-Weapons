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

using MB;

namespace Game
{
	public class WeaponPositionRecoil : WeaponRecoil.Effect
    {
        protected override void Reset()
        {
            base.Reset();

            kick = new ValueRange(-0.05f, -0.075f);
            sway = new SwayData(0.01f);
            speed = new SpeedData(10, 20);

            noise.Weight = 0.6f;
        }

        protected override void CalculateTarget()
        {
            Target = new Vector3()
            {
                x = noise.Lerp(5, sway.Horizontal),
                y = noise.Lerp(3, sway.Vertical),
                z = noise.Lerp(1, kick),
            };

            Target *= Recoil.Scale.Value;
        }

        protected override void Write() => Context.localPosition += Value;
    }
}