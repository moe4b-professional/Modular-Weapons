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
    [RequireComponent(typeof(ControllerTransformAnchor))]
	public class ControllerCamera : FirstPersonController.Module
	{
		public Camera Component { get; protected set; }

        public ControllerTransformAnchor Anchor { get; protected set; }

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

            Anchor = GetComponent<ControllerTransformAnchor>();
            
            Modules = new Modules.Collection<ControllerCamera>(this);
            Modules.Register(Controller.Behaviours);

            FOV = Modules.Depend<ControllerCameraFOV>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }
    }
}