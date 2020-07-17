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
	public class WeaponIdleMotionRotation : WeaponIdleMotionEffect
    {
		[SerializeField]
        [LabeledVector(VectorLabels.Rotation)]
        protected Vector3 range = new Vector3(0.2f, 0.6f, 0.4f);
        public Vector3 Range { get { return range; } }

        protected override void CalculateOffset()
        {
            Offset = Vector3.Scale(IdleMotion.Target, range);

            Offset *= IdleMotion.Scale.Value;
        }

        protected override void Apply()
        {
            Context.Rotate(Vector3.right, Offset.x, Space.Self);
            Context.Rotate(Vector3.up, Offset.y, Space.Self);
            Context.Rotate(Vector3.forward, Offset.z, Space.Self);
        }
    }
}