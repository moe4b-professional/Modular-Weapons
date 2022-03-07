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
    public class WeaponReloadAimConstraint : WeaponReload.Module
    {
        public bool Active => enabled && Reload.IsProcessing;

        public bool Modifier() => Active;

        [field: SerializeField, DebugOnly]
        public WeaponAim Aim { get; protected set; }

        public override void Set(WeaponReload value)
        {
            base.Set(value);

            Aim = Weapon.Modules.Depend<WeaponAim>();
        }

        public override void Initialize()
        {
            base.Initialize();

            Aim.Constraint.Add(Modifier);
        }
    }
}