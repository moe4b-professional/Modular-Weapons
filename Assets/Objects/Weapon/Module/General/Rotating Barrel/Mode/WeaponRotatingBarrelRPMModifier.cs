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
	public class WeaponRotatingBarrelRPMModifier : WeaponRotatingBarrel.Module
    {
        public float Modifier() => RotatingBarrel.Rate;

        public WeaponRPM RPM { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            RPM = Weapon.Modules.Depend<WeaponRPM>();
        }

        public override void Initialize()
        {
            base.Initialize();

            RPM.Scale.Add(Modifier);
        }
    }
}