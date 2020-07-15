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
        protected override void Reset()
        {
            base.Reset();

            range = 0.0035f;
        }

        protected override void Process()
        {
            base.Process();

            var Offset = Bob.Processor.Delta;

            Offset *= range * Bob.Scale.Value;

            Context.localPosition += Offset;
        }
    }
}