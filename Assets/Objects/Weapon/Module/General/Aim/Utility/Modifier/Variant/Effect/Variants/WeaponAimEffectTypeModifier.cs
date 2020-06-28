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
	public class WeaponAimEffectTypeModifier : WeaponAimEffectModifier
    {
#if UNITY_EDITOR
        [SerializeField]
        protected MonoScript[] types;
        public MonoScript[] Types { get { return types; } }
#endif

        [SerializeField]
        [HideInInspector]
        string[] IDs;

        protected virtual void OnValidate()
        {
            if(types == null || types.Length == 0)
                IDs = new string[0];
            else
                IDs = new string[types.Length];

            for (int i = 0; i < IDs.Length; i++)
            {
                if(types[i] == null)
                {
                    IDs[i] = string.Empty;
                }
                else
                {
                    var type = types[i].GetClass();

                    IDs[i] = type.AssemblyQualifiedName;
                }
            }
        }

        public override bool IsTarget(WeaponEffects.IInterface effect)
        {
            if (IDs == null) return false;

            var ID = effect.GetType().AssemblyQualifiedName;

            return IDs.Contains(ID);
        }
    }
}