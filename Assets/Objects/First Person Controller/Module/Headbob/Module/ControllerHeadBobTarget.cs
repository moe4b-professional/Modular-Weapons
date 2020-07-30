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
	public class ControllerHeadBobTarget : ControllerHeadBob.Module
	{
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        [SerializeField]
        protected bool invert = false;
        public bool Invert { get { return invert; } }

        [SerializeField]
        protected float scale = 1f;
        public float Scale { get { return scale; } }

        public override void Init()
        {
            base.Init();

            anchor.OnWriteDefaults += Write;
        }

        public virtual void Write()
        {
            var value = HeadBob.Offset;

            if (invert) value = -value;

            value *= scale;

            anchor.LocalPosition += value;
        }
    }
}