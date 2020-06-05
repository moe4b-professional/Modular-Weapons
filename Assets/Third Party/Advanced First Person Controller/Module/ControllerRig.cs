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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public class ControllerRig : FirstPersonController.Module
	{
        [SerializeField]
        protected CameraData _camera;
        public CameraData camera { get { return _camera; } }
        [Serializable]
        public class CameraData
        {
            [SerializeField]
            protected ControllerMotionEffectTransform pivot;
            public ControllerMotionEffectTransform Pivot { get { return pivot; } }

            [SerializeField]
            protected ControllerMotionEffectTransform anchor;
            public ControllerMotionEffectTransform Anchor { get { return anchor; } }

            public ControllerCamera Module => Rig.Controller.camera;

            public ControllerRig Rig { get; protected set; }
            public virtual void Configure(ControllerRig reference)
            {
                Rig = reference;
            }
        }

        public override void Configure()
        {
            base.Configure();

            camera.Configure(this);
        }
    }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}