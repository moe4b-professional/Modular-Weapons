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
        [SerializeField]
        [LabeledVector(VectorLabels.Position)]
        protected Vector3 effect = new Vector3(0.01f, 0.02f, 0.03f);
        public override Vector3 Effect => effect;

        protected override void Reset()
        {
            base.Reset();

            multiplier = 0.2f;
        }

        protected override void CalculateOffset()
        {
            Offset = Vector3.zero;

            Offset += Vector3.right * effect.x * Sway.Value.x;
            Offset += Vector3.up * effect.y * Sway.Value.y;

            Offset *= multiplier * Sway.Scale.Value;
        }

        protected override void Apply()
        {
            Context.Translate(Offset, Anchor);
        }
    }
}