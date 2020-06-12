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
    public class PlayerWeaponCameraBlurProcessor : PlayerWeaponProcessor, WeaponCameraBlur.IProcessor, Modifier.Average.IInterface
    {
        public float Value { get; set; }

        public override void Init()
        {
            base.Init();

            Player.CameraEffects.Blur.Average.Register(this);
        }
    }
}