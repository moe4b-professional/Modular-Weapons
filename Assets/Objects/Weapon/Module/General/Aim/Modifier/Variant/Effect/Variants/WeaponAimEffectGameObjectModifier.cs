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
	public class WeaponAimEffectGameObjectModifier : WeaponAimEffectModifier
    {
        [SerializeField]
        protected GameObject source;
        public GameObject Source { get { return source; } }

        public override bool IsTarget(WeaponEffects.IInterface effect) => effect.transform.IsChildOf(source.transform);
    }
}