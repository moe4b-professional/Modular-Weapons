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
        protected float range = 0.0035f;
        public float Range { get { return range; } }

        [SerializeField]
        protected CurvesData curves;
        public CurvesData Curves { get { return curves; } }
        [Serializable]
        public class CurvesData
        {
            [SerializeField]
            protected AnimationCurve vertical;
            public AnimationCurve Vertical { get { return vertical; } }

            [SerializeField]
            protected AnimationCurve horizontal;
            public AnimationCurve Horizontal { get { return horizontal; } }

            public virtual Vector3 Evaluate(float time)
            {
                return Vector3.up * vertical.Evaluate(time) + Vector3.right * horizontal.Evaluate(time);
            }
        }

        public Vector3 Delta { get; protected set; }

        public Vector3 Offset { get; protected set; }

        public class Module : FirstPersonController.BaseModule<ControllerHeadBob>
        {
            public ControllerHeadBob HeadBob => Reference;

            public override FirstPersonController Controller => HeadBob.Controller;
        }

        public References.Collection<ControllerHeadBob> Modules { get; protected set; }

        public ControllerStep Step => Controller.Step;

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Modules = new References.Collection<ControllerHeadBob>(this, Controller.gameObject);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;

            Modules.Init();
        }

        void Process()
        {
            Delta = curves.Evaluate(Step.Rate);

            Delta *= Step.Weight.Value;

            Offset = Delta * range;
        }
    }
}