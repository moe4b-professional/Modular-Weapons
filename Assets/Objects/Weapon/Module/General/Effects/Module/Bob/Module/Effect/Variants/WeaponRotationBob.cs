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
	public class WeaponRotationBob : WeaponBobEffect
    {
        protected override void Reset()
        {
            base.Reset();

            range = 1f;
        }

        protected override void Process()
        {
            base.Process();

            var Offset = Vector3.forward * Bob.Processor.Delta.x;

            Offset *= range * Bob.Scale.Value;

            Context.localEulerAngles += Offset;
        }
    }
}