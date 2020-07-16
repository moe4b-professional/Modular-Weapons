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
	public class WeaponPositionBob : WeaponBobEffect
    {
        [SerializeField]
        protected float range = 0.0035f;
        public float Range { get { return range; } }

        protected override void CalculateOffset()
        {
            if(enabled)
            {
                Offset = Processor.Delta;

                Offset *= range * Bob.Scale.Value;
            }
        }

        protected override void Apply()
        {
            Context.localPosition += Offset;
        }
    }
}