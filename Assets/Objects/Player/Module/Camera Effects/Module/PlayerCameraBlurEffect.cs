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

using UnityEngine.Rendering.PostProcessing;

namespace Game
{
	public class PlayerCameraBlurEffect : PlayerCameraEffects.Module
	{
        [SerializeField]
        protected ValueRange range;
        public ValueRange Range { get { return range; } }

        public float Scale
        {
            set
            {
                Settings.focalLength.value = value;
            }
        }

        public DepthOfField Settings { get; protected set; }

        public Modifier.Average Average { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Average = new Modifier.Average();
        }

        public override void Init()
        {
            base.Init();

            Settings = CameraEffects.Profile.GetSetting<DepthOfField>();

            if (Settings == null)
                throw new Exception("Dependancy Missing");

            Player.OnProcess += Process;
        }

        void Process()
        {
            Scale = range.Lerp(Average.Value);
        }
    }
}