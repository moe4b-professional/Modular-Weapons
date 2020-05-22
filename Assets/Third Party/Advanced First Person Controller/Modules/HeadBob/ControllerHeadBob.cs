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
	public class ControllerHeadBob : FirstPersonController.Module
	{
        [SerializeField]
        protected float scale = 1f;
        public float Scale { get { return scale; } }

        [SerializeField]
        protected AnimationCurve curve;
        public AnimationCurve Curve { get { return curve; } }

        [SerializeField]
        protected float range = 0.2f;
        public float Range { get { return range; } }

        public ControllerStep Step => Controller.Step;

        public Vector3 Offset { get; protected set; }

        public class Module : FirstPersonController.Module
        {
            public ControllerHeadBob HeadBob => Controller.HeadBob;
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Offset = Vector3.up * curve.Evaluate(Step.Rate) * range * scale;
        }
    }
}