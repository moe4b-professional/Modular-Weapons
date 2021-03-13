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
    [RequireComponent(typeof(TransformAnchor))]
	public class ControllerCamera : FirstPersonController.Module
	{
		public Camera Component { get; protected set; }

        public TransformAnchor Anchor { get; protected set; }

        public ControllerCameraFOV FOV { get; protected set; }

        public Modules<ControllerCamera> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerCamera>
        {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
            public ControllerCamera camera { get; protected set; }
            public virtual void Set(ControllerCamera value) => camera = value;

            public FirstPersonController Controller => camera.Controller;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        }

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Modules = new Modules<ControllerCamera>(this);
            Modules.Register(Controller.Behaviours);

            FOV = Modules.Depend<ControllerCameraFOV>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Component = GetComponent<Camera>();

            Anchor = GetComponent<TransformAnchor>();
        }
    }
}