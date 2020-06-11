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
	public class WeaponAimSensitivtyModifier : WeaponAimPropertyModifier
	{
        public WeaponSensitivty Sensitivty { get; protected set; }

        protected override void Reset()
        {
            base.Reset();

            scale = new ValueRange(0.6f, 1f);
        }

        public override void Configure()
        {
            base.Configure();

            Sensitivty = Weapon.Modules.Find<WeaponSensitivty>();

            if (Sensitivty == null)
                ExecuteDependancyError<WeaponSensitivty>();
        }

        public override void Init()
        {
            base.Init();

            Sensitivty.Scale.Register(this);
        }
    }
}