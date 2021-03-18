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
        protected GameObject[] source;
        public GameObject[] Source { get { return source; } }

        public override void Configure()
        {
            base.Configure();

            for (int i = 0; i < source.Length; i++)
            {
                var instances = Dependancy.GetAll<WeaponEffects.IInterface>(source[i]);

                Targets.AddRange(instances);
            }
        }

        public override bool IsTarget(WeaponEffects.IInterface effect) => Targets.Contains(effect);
    }
}