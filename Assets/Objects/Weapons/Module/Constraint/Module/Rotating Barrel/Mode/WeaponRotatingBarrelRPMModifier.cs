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
	public class WeaponRotatingBarrelRPMModifier : WeaponRotatingBarrel.Module, Modifier.Scale.IInterface
    {
        float Modifier.Scale.IInterface.Value => RotatingBarrel.Rate;

        public WeaponRPM RPM { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            RPM = Weapon.Modules.Depend<WeaponRPM>();
        }

        public override void Init()
        {
            base.Init();

            RPM.Scale.Register(this);
        }
    }
}