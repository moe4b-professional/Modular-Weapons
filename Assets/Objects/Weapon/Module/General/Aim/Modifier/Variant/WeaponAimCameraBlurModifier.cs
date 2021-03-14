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
	public class WeaponAimCameraBlurModifier : WeaponAimPropertyModifier
	{
        [SerializeField]
        protected float target = 0.6f;
        public float Target { get { return target; } }

        public virtual float Value => Mathf.Lerp(0f, target, Rate);

        public float Modifier() => Value;

        public WeaponCameraBlur CameraBlur { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            CameraBlur = Weapon.Modules.Depend<WeaponCameraBlur>();
        }

        public override void Init()
        {
            base.Init();

            CameraBlur.Average.Add(Modifier);
        }
    }
}