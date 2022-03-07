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
    public class WeaponSprintAimConstraint : WeaponSprint.Module
    {
        [SerializeField]
        protected bool clearInput = true;
        public bool ClearInput { get { return clearInput; } }

        public bool Active => enabled && Sprint.Active;

        public bool Modifier() => Active;

        [field: SerializeField, DebugOnly]
        public WeaponAim Aim { get; protected set; }

        public override void Set(WeaponSprint value)
        {
            base.Set(value);

            Aim = Weapon.Modules.Depend<WeaponAim>();
        }

        public override void Initialize()
        {
            base.Initialize();

            Aim.Constraint.Add(Modifier);

            Sprint.OnBegin += SprintBeginCallback;
        }

        void SprintBeginCallback()
        {
            if(enabled && clearInput)
                Aim.ClearInput();
        }
    }
}