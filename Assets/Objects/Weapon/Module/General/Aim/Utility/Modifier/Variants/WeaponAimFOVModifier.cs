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
	public class WeaponAimFOVModifier : WeaponAimPropertyModifier
    {
        public WeaponFOV FOV { get; protected set; }

        protected override void Reset()
        {
            base.Reset();

            range = new ValueRange(0.8f, 1f);
        }

        public override void Configure()
        {
            base.Configure();

            FOV = Weapon.Modules.Find<WeaponFOV>();

            if (FOV == null)
                ExecuteDependancyError<WeaponFOV>();
        }

        public override void Init()
        {
            base.Init();

            FOV.Scale.Register(this);
        }
    }
}