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
	public class WeaponAimSensitivtyModifier : WeaponAimPropertyModifier, Modifier.Scale.IInterface
	{
        [SerializeField]
        protected float scale = 0.6f;
        public float Scale { get { return scale; } }

        public virtual float Value => Mathf.Lerp(1f, scale, Rate);

        public WeaponSensitivty Sensitivty { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Sensitivty = Weapon.Modules.Depend<WeaponSensitivty>();
        }

        public override void Init()
        {
            base.Init();

            Sensitivty.Scale.Register(this);
        }
    }
}