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

using MB;

namespace Game
{
	public class PlayerCameraBlurEffect : PlayerCameraEffects.Module
	{
        [SerializeField]
        protected CameraBlur component;
        public CameraBlur Component { get { return component; } }

        public Modifier.Average Average { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Average = new Modifier.Average();
        }
        public override void Initialize()
        {
            base.Initialize();
            
            Player.OnProcess += Process;
        }

        void Process()
        {
            component.Scale = Average.Value;
        }
    }
}