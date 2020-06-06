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
	public class ControllerCameraRelay : ControllerCamera.Module
	{
		public Camera Component { get; protected set; }

        public float InitialFOV { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Component = GetComponent<Camera>();

            InitialFOV = Component.fieldOfView;
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Component.fieldOfView = InitialFOV * camera.FOV.Scale.Value;
        }
    }
}