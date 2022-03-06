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
	public class ControllerCameraRelay : ControllerCamera.Module
	{
        [field: SerializeField, DebugOnly]
		public Camera Component { get; protected set; }

        public float InitialFOV { get; protected set; }

        public override void Set(ControllerCamera value)
        {
            base.Set(value);

            Component = GetComponent<Camera>();
        }

        public override void Configure()
        {
            base.Configure();

            InitialFOV = Component.fieldOfView;
        }
        public override void Initialize()
        {
            base.Initialize();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Component.fieldOfView = InitialFOV * camera.FOV.Scale.Value;
        }
    }
}