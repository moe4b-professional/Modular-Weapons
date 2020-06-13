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
	public class WeaponAimSpeedModifier : WeaponAimPropertyModifier
	{
        public WeaponAimSpeed Speed => Aim.Speed;

        protected override void Reset()
        {
            base.Reset();

            range = new ValueRange(0.75f, 1f);
        }

        public override void Init()
        {
            base.Init();

            Speed.Scale.Register(this);
        }
    }
}