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
        protected TransformAnchor pivot;
        public TransformAnchor Pivot { get { return pivot; } }

        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        public ControllerCamera camera => Controller.camera;
        public ControllerCamera Camera => camera; //Don't judge me, I want to maintain my PascalCase naming while still maintaining a propert override to camera
    }
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
}