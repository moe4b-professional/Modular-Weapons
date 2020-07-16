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
        [SerializeField]
        [LabeledVector(VectorLabels.Rotation)]
        protected Vector3 effect;
        public override Vector3 Effect => effect;

        protected override void Reset()
        {
            base.Reset();

            multiplier = 1f;
            effect = new Vector3(10, 5, 5);
        }

        protected override void CalculateOffset()
        {
            Offset = Vector3.zero;

            if(enabled)
            {
                Offset += Vector3.right * effect.x * -Sway.Value.y;
                Offset += Vector3.up * effect.y * Sway.Value.x;
                Offset += Vector3.forward * effect.z * Sway.Value.x;

                Offset *= multiplier * Sway.Scale.Value;
            }
        }

        protected override void Apply()
        {
            Context.Rotate(Anchor.up, Offset.y, Space.World);
            Context.Rotate(Anchor.right, Offset.x, Space.World);
            Context.Rotate(Anchor.forward, Offset.z, Space.World);
        }
    }
}