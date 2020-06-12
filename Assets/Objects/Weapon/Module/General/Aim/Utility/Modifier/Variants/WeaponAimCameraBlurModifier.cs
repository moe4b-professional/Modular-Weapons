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
        public WeaponCameraBlur CameraBlur { get; protected set; }

        protected override void Reset()
        {
            base.Reset();

            scale = new ValueRange(1f, 0f);
        }

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