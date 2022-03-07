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
using MB;

namespace Game
{
	public class WeaponAimCameraBlurModifier : WeaponAimPropertyModifier
	{
        [SerializeField]
        protected float target = 0.6f;
        public float Target { get { return target; } }

        public virtual float Value => Mathf.Lerp(0f, target, Rate);

        public float Modifier() => Value;

        [field: SerializeField, DebugOnly]
        public WeaponCameraBlur CameraBlur { get; protected set; }

        public override void Set(WeaponAim value)
        {
            base.Set(value);

            CameraBlur = Weapon.Modules.Depend<WeaponCameraBlur>();
        }

        public override void Initialize()
        {
            base.Initialize();

            CameraBlur.Average.Add(Modifier);
        }
    }
}