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
	public class WeaponAimFOVModifier : WeaponAimPropertyModifier, Modifier.Scale.IInterface
    {
        [SerializeField]
        protected float scale = 0.8f;
        public float Scale { get { return scale; } }

        public virtual float Value => Mathf.Lerp(1f, scale, Rate);

        public WeaponFOV FOV { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            FOV = Weapon.Modules.Depend<WeaponFOV>();
        }

        public override void Init()
        {
            base.Init();

            FOV.Scale.Register(this);
        }
    }
}