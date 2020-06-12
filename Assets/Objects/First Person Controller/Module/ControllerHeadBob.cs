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
        protected float range = 0.05f;
        public float Range { get { return range; } }

        [SerializeField]
        protected ScaleData scale;
        public ScaleData Scale { get { return scale; } }
        [Serializable]
        public class ScaleData
        {
            [SerializeField]
            protected float vertical = 0.5f;
            public float Vertical { get { return vertical; } }

            [SerializeField]
            protected float horizontal = 1f;
            public float Horizontal { get { return horizontal; } }
        }

        [SerializeField]
        protected float speed = 4f;
        public float Speed { get { return speed; } }

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
            Calculate();

            for (int i = 0; i < contexts.Length; i++)
                contexts[i].Apply(Offset);
        }

        protected virtual void Calculate()
        {
            Delta = new Vector3()
            {
                x = scale.Horizontal * Mathf.Sin(speed * Step.Time),
                y = scale.Vertical * Mathf.Sin(speed * 2 * Step.Time)
            };

            Delta *= Step.Weight.Value;

            Offset = Delta * range;
        }
    }
}