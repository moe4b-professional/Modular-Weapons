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
	public class WeaponAimSpeedModifier : WeaponAimPropertyModifier, Modifier.Scale.IInterface
	{
        [SerializeField]
        protected float scale = 0.75f;
        public float Scale { get { return scale; } }

        public override EffectMode Effect => EffectMode.Constant;

        public override float Value => Mathf.Lerp(1f, scale, Rate);

        public WeaponAimSpeed Speed => Aim.Speed;

        public override void Init()
        {
            base.Init();

            Speed.Scale.Register(this);
        }
    }
}