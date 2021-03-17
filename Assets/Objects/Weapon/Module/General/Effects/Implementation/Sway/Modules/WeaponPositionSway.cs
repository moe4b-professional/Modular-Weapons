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

using UnityEngine.Serialization;

namespace Game
{
	public class WeaponPositionSway : WeaponSway.Effect
    {
        [SerializeField]
        [FormerlySerializedAs("effect")]
        [LabeledVector(VectorLabels.Position)]
        protected Vector3 scale = new Vector3(0.01f, 0.02f, 0.03f);
        public override Vector3 Scale => scale;

        protected override void Reset()
        {
            base.Reset();

            multiplier = 0.2f;
        }

        protected override void CalculateOffset()
        {
            Offset = Vector3.zero;

            if (enabled)
            {
                Offset += Vector3.left * scale.x * Sway.Value.x;
                Offset += Vector3.down * scale.y * Sway.Value.y;

                Offset *= multiplier * Sway.Scale.Value;
            }
        }

        protected override void Write()
        {
            Anchor.transform.Translate(Offset, Axis);
        }
    }
}