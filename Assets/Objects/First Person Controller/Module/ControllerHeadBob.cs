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
        protected float range = 0.028f;
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

        [SerializeField]
        protected ContextData[] contexts;
        public ContextData[] Contexts { get { return contexts; } }
        [Serializable]
        public class ContextData
        {
            [SerializeField]
            protected ControllerTransformAnchor anchor;
            public ControllerTransformAnchor Anchor { get { return anchor; } }

            [SerializeField]
            protected bool invert = false;
            public bool Invert { get { return invert; } }

            [SerializeField]
            protected float scale = 1f;
            public float Scale { get { return scale; } }

            public virtual void Apply(Vector3 value)
            {
                if (invert) value = -value;

                value *= scale;

                anchor.LocalPosition += value;
            }
        }

        public Vector3 Delta { get; protected set; }

        public Vector3 Offset { get; protected set; }

        public ControllerStep Step => Controller.Step;

        public override void Init()
        {
            base.Init();

            Controller.Anchors.OnLateProcess += LateProcess;
        }

        void LateProcess()
        {
            Delta = curves.Evaluate(Step.Rate);

            Delta *= Step.Weight.Value;

            Offset = Delta * range;

            for (int i = 0; i < contexts.Length; i++)
                contexts[i].Apply(Offset);
        }
    }
}