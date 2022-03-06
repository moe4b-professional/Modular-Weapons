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
    [RequireComponent(typeof(Camera))]
    [RequireComponent(typeof(TransformAnchor))]
	public class ControllerCamera : FirstPersonController.Module
	{
        [field: SerializeField, DebugOnly]
        public Camera Component { get; protected set; }

        [field: SerializeField, DebugOnly]
        public TransformAnchor Anchor { get; protected set; }

        [field: SerializeField, DebugOnly]
        public ControllerCameraFOV FOV { get; protected set; }

        [field: SerializeField, DebugOnly]
        public Modules<ControllerCamera> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerCamera>
        {
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
            [field: SerializeField, DebugOnly]
            public ControllerCamera camera { get; protected set; }

            public FirstPersonController Controller => camera.Controller;

            public virtual void Set(ControllerCamera value) => camera = value;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        }

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Component = GetComponent<Camera>();
            Anchor = GetComponent<TransformAnchor>();

            Modules = new Modules<ControllerCamera>(this);
            Modules.Register(Controller.Behaviours);

            FOV = Modules.Depend<ControllerCameraFOV>();

            Modules.Set();
        }
    }
}