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
    [RequireComponent(typeof(ManualCameraRender))]
	public class WeaponAimSightManualCameraRenderOptimization : WeaponAimSight.Module
	{
        [SerializeField]
        protected int minFPS = 20;
        public int MinFPS { get { return minFPS; } }

        public int MaxFPS { get; protected set; }

        public int TargetFPS
        {
            get
            {
                if (Sight.Aim.IsOn && Sight.enabled) return MaxFPS;

                var value = Mathf.Lerp(MinFPS, MaxFPS, Sight.Weight);

                return Mathf.RoundToInt(value);
            }
        }

        [field: SerializeField, DebugOnly]
        public ManualCameraRender Render { get; protected set; }

        public override void Set(WeaponAimSight value)
        {
            base.Set(value);

            Render = GetComponent<ManualCameraRender>();
        }

        public override void Configure()
        {
            base.Configure();

            MaxFPS = Render.FPS;
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            Render.FPS = TargetFPS;
        }
    }
}