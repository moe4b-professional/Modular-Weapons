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
	public class WeaponRotationBob : WeaponBob.Effect
    {
        [SerializeField]
        [LabeledVector(VectorLabels.Rotation)]
        protected Vector3 range = new Vector3(1f, 0.5f, 0.5f);
        public Vector3 Range { get { return range; } }

        protected override void CalculateOffset()
        {
            Offset = Vector3.zero;

            if(enabled)
            {
                Offset += Vector3.right * Processor.Delta.y;
                Offset += Vector3.up * -Processor.Delta.x;
                Offset += Vector3.forward * (Processor.Delta.x + Processor.Delta.y);

                Offset = Vector3.Scale(Offset, range);

                Offset *= Bob.Scale.Value;
            }
        }

        protected override void Write()
        {
            Context.localEulerAngles += Offset;
        }
    }
}