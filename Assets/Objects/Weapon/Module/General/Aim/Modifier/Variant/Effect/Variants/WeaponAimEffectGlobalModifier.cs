﻿using System;
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
    public class WeaponAimEffectGlobalModifier : WeaponAimEffectModifier
    {
        [field: SerializeField, DebugOnly]
        public List<WeaponAimEffectModifier> Overrides { get; protected set; }

        public override void Set(WeaponAim value)
        {
            base.Set(value);

            Overrides = Aim.Modules.FindAll<WeaponAimEffectModifier>();
            Overrides.RemoveAll(x => x is WeaponAimEffectGlobalModifier);
        }

        public override bool IsTarget(WeaponEffects.IInterface effect)
        {
            for (int i = 0; i < Overrides.Count; i++)
                if (Overrides[i].IsTarget(effect))
                    return false;

            return true;
        }
    }
}