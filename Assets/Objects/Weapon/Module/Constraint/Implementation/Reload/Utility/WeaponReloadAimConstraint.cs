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
    public class WeaponReloadAimConstraint : WeaponReload.Module, Modifier.Constraint.IInterface
    {
        public bool Active => enabled && Reload.IsProcessing;

        public WeaponAim Aim { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Aim = Weapon.Modules.Find<WeaponAim>();

            if (Aim == null)
                ExecuteDependancyError<WeaponAim>();
        }

        public override void Init()
        {
            base.Init();

            Aim.Constraint.Register(this);
        }
    }
}