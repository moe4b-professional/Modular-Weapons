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
    [RequireComponent(typeof(TransformAnchor))]
    public class ControllerTransformAnchor : ControllerAnchors.Module, ControllerAnchors.IInterface
	{
        public Vector3 LocalPosition
        {
            get => transform.localPosition;
            set => transform.localPosition = value;
        }

        public Vector3 LocalAngles
        {
            get => transform.localEulerAngles;
            set => transform.localEulerAngles = value;
        }

        public Quaternion LocalRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }

        public TransformAnchor Component { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Component = GetComponent<TransformAnchor>();
            Component.Configure();
        }

        public override void Init()
        {
            base.Init();

            Anchors.Register(this);
        }

        void Process()
        {
            Component.WriteDefaults();
        }

        public virtual void WriteDefaults() => Component.WriteDefaults();
    }
}