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
	public class WeaponPositionSway : WeaponSwayEffect
    {
        protected override void Reset()
        {
            base.Reset();

            multiplier = 1f;
            effect = new EffectData(10, 5, 5);
        }

        protected override void SampleOffset()
        {
            Offset = Vector3.zero;

            Offset += Vector3.right * effect.Horizontal * Sway.Value.x;
            Offset += Vector3.up * effect.Vertical * Sway.Value.y;
        }

        protected override void Apply() => Context.localPosition += Offset;
    }
}