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
	public class WeaponIdleMotionPosition : WeaponIdleMotion.Effect
    {
        [SerializeField]
        [LabeledVector(VectorLabels.Position)]
        protected Vector3 range = new Vector3(0.002f, 0.002f, 0.001f);
        public Vector3 Range { get { return range; } }

        protected override void CalculateOffset()
        {
            Offset = Vector3.Scale(IdleMotion.Target, range);

            Offset *= IdleMotion.Scale.Value;
        }

        protected override void Write()
        {
            Context.localPosition += Offset;
        }
    }
}