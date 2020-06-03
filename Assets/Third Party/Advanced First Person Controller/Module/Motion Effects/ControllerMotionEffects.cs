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
	public class ControllerMotionEffects : FirstPersonController.Module
	{
		public class Module : FirstPersonController.BaseModule<ControllerMotionEffects>
        {
            public ControllerMotionEffects MotionEffects => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }

        public References.Collection<ControllerMotionEffects> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modules = new References.Collection<ControllerMotionEffects>(this, Controller.gameObject);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            Modules.Init();
        }

        public event Action OnEarlyProcess;
        public event Action OnProcess;
        void Process()
        {
            OnEarlyProcess?.Invoke();
            OnProcess?.Invoke();
        }
    }
}