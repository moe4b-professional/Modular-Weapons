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
	public class WeaponRotationSway : WeaponSway.Effect
    {
        [SerializeField]
        [FormerlySerializedAs("effect")]
        [LabeledVector(VectorLabels.Rotation)]
        protected Vector3 scale = new Vector3(10, 5, 5);
        public override Vector3 Scale => scale;

        protected override void Reset()
        {
            base.Reset();

            multiplier = 1f;
        }

        protected override void CalculateOffset()
        {
            Offset = Vector3.zero;

            if(enabled)
            {
                Offset += Vector3.right * scale.x * -Sway.Value.y;
                Offset += Vector3.up * scale.y * Sway.Value.x;
                Offset += Vector3.forward * scale.z * Sway.Value.x;

                Offset *= multiplier * Sway.Scale.Value;
            }
        }

        protected override void Write()
        {
            Context.Rotate(Anchor.right, Offset.x, Space.World);
            Context.Rotate(Anchor.up, Offset.y, Space.World);
            Context.Rotate(Anchor.forward, Offset.z, Space.World);
        }
    }
}