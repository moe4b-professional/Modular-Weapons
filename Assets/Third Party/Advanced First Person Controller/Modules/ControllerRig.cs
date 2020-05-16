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
        protected Transform pivot;
        public Transform Pivot { get { return pivot; } }

        [SerializeField]
        protected CameraData _camera;
        public CameraData camera { get { return _camera; } }
        [Serializable]
        public class CameraData
        {
            [SerializeField]
            protected Transform pivot;
            public Transform Pivot { get { return pivot; } }

            [SerializeField]
            protected Camera component;
            public Camera Component { get { return component; } }
        }
    }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}