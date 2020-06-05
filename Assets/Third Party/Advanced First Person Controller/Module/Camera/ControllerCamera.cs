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
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(ControllerMotionEffectTransform))]
	public class ControllerCamera : FirstPersonController.Module
	{
		public Camera Component { get; protected set; }

        public ControllerMotionEffectTransform MotionEffectTransform { get; protected set; }

        public ControllerCameraFOV FOV { get; protected set; }

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public class Module : FirstPersonController.BaseModule<ControllerCamera>
        {
            public ControllerCamera camera => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        public Modules.Collection<ControllerCamera> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Component = GetComponent<Camera>();

            MotionEffectTransform = GetComponent<ControllerMotionEffectTransform>();
            
            Modules = new Modules.Collection<ControllerCamera>(this);

            FOV = Modules.Find<ControllerCameraFOV>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }
    }
}