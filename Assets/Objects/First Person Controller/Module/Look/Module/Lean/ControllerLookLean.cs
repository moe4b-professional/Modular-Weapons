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
	public class ControllerLookLean : ControllerLook.Module
	{
		[SerializeField]
        protected float range = 40f;
        public float Range { get { return range; } }

        [SerializeField]
        protected float speed = 3f;
        public float Speed { get { return speed; } }

        public float Target { get; protected set; }

        public float Rate { get; protected set; }

        public float Angle => range * Rate;

        [SerializeField]
        protected InputMode mode = InputMode.Hold;
        public InputMode Mode { get { return mode; } }
        public enum InputMode
        {
            Hold, Toggle
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
            [Range(0f, 1f)]
            protected float scale = 1f;
            public float Scale { get { return scale; } }

            public virtual void Apply(Quaternion offset)
            {
                if (invert) offset = Quaternion.Inverse(offset);

                offset = Quaternion.Lerp(Quaternion.identity, offset, scale);

                anchor.LocalRotation *= offset;
            }
        }

        public Vector3 Axis => Vector3.forward;

        public Quaternion Offset { get; protected set; }

        public class Module : FirstPersonController.BaseModule<ControllerLookLean>
        {
            public ControllerLookLean Lean => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }

        public Modules.Collection<ControllerLookLean> Modules { get; protected set; }

        public AxisInput Input => Controller.Input.Lean;

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<ControllerLookLean>(this);
            Modules.Register(Controller.Behaviours);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            CalculateOffset();

            Controller.Anchors.OnLateProcess += LateProcess;

            Modules.Init();
        }

        void LateProcess()
        {
            CalculateTarget();

            Rate = Mathf.MoveTowards(Rate, Target, speed * Time.deltaTime);

            CalculateOffset();

            for (int i = 0; i < contexts.Length; i++)
                contexts[i].Apply(Offset);
        }

        protected virtual void CalculateTarget()
        {
            ProcessButton(Input.Positive, 1f);

            ProcessButton(Input.Negative, -1f);
        }

        protected virtual void ProcessButton(ButtonInput button, float value)
        {
            if(mode == InputMode.Hold)
            {
                if (button.Press)
                    Target = value;
                if (button.Up)
                    Target = 0f;
            }

            if(mode == InputMode.Toggle)
            {
                if(button.Press)
                {
                    if (Target == value)
                        Target = 0f;
                    else if (Target == 0f)
                        Target = value;
                    else
                        Target = 0f;
                }
            }
        }

        public virtual void Stop()
        {
            Target = 0f;
        }

        protected virtual void CalculateOffset()
        {
            Offset = Quaternion.Euler(Axis * -Angle);
        }
    }
}