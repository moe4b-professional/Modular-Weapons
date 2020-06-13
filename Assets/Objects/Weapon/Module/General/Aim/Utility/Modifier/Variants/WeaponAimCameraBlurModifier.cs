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
	public class WeaponAimCameraBlurModifier : WeaponAimPropertyModifier, Modifier.Average.IInterface
	{
        [SerializeField]
        protected float target = 0.6f;
        public float Target { get { return target; } }

        public override float Value => Mathf.Lerp(0f, target, Rate);

        public WeaponCameraBlur CameraBlur { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            CameraBlur = Weapon.Modules.Find<WeaponCameraBlur>();

            if (CameraBlur == null)
                ExecuteDependancyError<WeaponCameraBlur>();
        }

        public override void Init()
        {
            base.Init();

            CameraBlur.Average.Register(this);
        }
    }
}