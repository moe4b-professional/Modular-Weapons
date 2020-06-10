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
	public class ControllerCameraFOV : ControllerCamera.Module
	{
		public float Initial { get; protected set; }

        public float Value
        {
            get => camera.Component.fieldOfView;
            set => camera.Component.fieldOfView = value;
        }

        public Modifier.Scale Scale { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Initial = Value;

            Scale = new Modifier.Scale();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Value = Initial * Scale.Value;
        }
    }
}