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
	public class ControllerLookLeanTarget : ControllerLookLean.Module
	{
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        [SerializeField]
        protected bool invert = false;
        public bool Invert { get { return invert; } }

        [SerializeField]
        [Range(0f, 1f)]
        protected float scale = 1f;
        public float Scale { get { return scale; } }

        public override void Initialize()
        {
            base.Initialize();

            anchor.OnWriteDefaults += Write;
        }

        public virtual void Write()
        {
            var value = Lean.Offset;

            if (invert) value = Quaternion.Inverse(value);

            value = Quaternion.Lerp(Quaternion.identity, value, scale);

            anchor.LocalRotation *= value;
        }
    }
}