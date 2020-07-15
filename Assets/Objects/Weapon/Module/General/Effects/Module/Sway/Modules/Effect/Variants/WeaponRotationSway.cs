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
	public class WeaponRotationSway : WeaponSwayEffect
	{
        protected override void Reset()
        {
            base.Reset();

            multiplier = 1f;
            effect = new EffectData(0.002f, 0.004f, 0.006f);
        }

        protected override void SampleOffset()
        {
            Offset = Vector3.zero;

            Offset += Vector3.forward * effect.Fordical * Sway.Value.x;
            Offset += Vector3.right * effect.Vertical * -Sway.Value.y;
            Offset += Vector3.up * effect.Horizontal * Sway.Value.x;
        }

        protected override void Apply() => Context.localEulerAngles += Offset;
    }
}