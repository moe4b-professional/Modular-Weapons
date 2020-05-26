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
    public class ControllerSprintStateElement : BaseControllerStateElement
    {
        [SerializeField]
        protected ControllerStateElement source;
        public ControllerStateElement Source { get { return source; } }

        [SerializeField]
        protected FloatToggleValue height;
        public override float Height => height.Evaluate(source.Height);

        [SerializeField]
        protected FloatToggleValue radius;
        public override float Radius => radius.Evaluate(source.Radius);

        [SerializeField]
        protected FloatToggleValue multiplier;
        public override float Multiplier => Mathf.Lerp(source.Multiplier, multiplier.Evaluate(source.Multiplier), Input.Sprint.Axis);

        [SerializeField]
        protected InputMode mode = InputMode.Toggle;
        public InputMode Mode { get { return mode; } }
        public enum InputMode
        {
            Toggle, Hold
        }

        public virtual bool CanDo
        {
            get
            {
                if (Vector3.Dot(Controller.Movement.Input.Relative, Vector3.forward) < 0.5f) return false;

                return true;
            }
        }

        protected override void Process()
        {
            base.Process();

            if(mode == InputMode.Hold)
            {
                if (Input.Sprint.Button.Press && CanDo)
                    Begin();

                if (Active && Input.Sprint.Button.Held == false)
                    End();
            }

            if(mode == InputMode.Toggle)
            {
                if (Input.Sprint.Button.Press)
                {
                    if (Active)
                        End();
                    else if (CanDo)
                        Begin();
                }
            }

            if (Active && CanDo == false)
                End();
        }

        protected virtual void Begin()
        {
            Transition.Set(this);
        }

        protected virtual void End()
        {
            Transition.Set(Sets.Normal);
        }
    }
}