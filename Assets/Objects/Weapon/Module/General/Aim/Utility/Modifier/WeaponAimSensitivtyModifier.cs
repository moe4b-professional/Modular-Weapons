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
	public class WeaponAimSensitivtyModifier : WeaponAim.Module, Modifier.Scale.IInterface
	{
        [SerializeField]
        protected ValueRange scale = new ValueRange(0.6f, 1f);
        public ValueRange Scale { get { return scale; } }

        public float Value => enabled ? scale.Lerp(Aim.InverseRate) : 1f;

        public WeaponSensitivty Sensitivity { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Sensitivity = Weapon.Modules.Find<WeaponSensitivty>();

            if (Sensitivity == null)
                ExecuteDependancyError<WeaponSensitivty>();
        }

        public override void Init()
        {
            base.Init();

            Sensitivity.Scale.Register(this);
        }
    }
}