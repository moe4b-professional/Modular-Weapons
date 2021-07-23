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
	public class WeaponRotationRecoil : WeaponRecoil.Effect
    {
        protected override void Reset()
        {
            base.Reset();

            kick = new ValueRange(-5, 2);
            sway = new SwayData(5);
            speed = new SpeedData(3, 5);

            noise.Weight = 0.3f;
        }
        
        protected override void CalculateTarget()
        {
            Target = new Vector3()
            {
                x = noise.Lerp(1, kick),
                y = noise.Lerp(3, sway.Vertical),
                z = noise.Lerp(5, sway.Horizontal),
            };

            Target *= Recoil.Scale.Value;
        }

        protected override void Write() => Context.localEulerAngles += Value;
    }
}