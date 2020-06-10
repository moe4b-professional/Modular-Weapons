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
    public class ControllerSprintStateElement : ControllerStateElement
    {
        [SerializeField]
        protected ControllerStateElement source;
        public ControllerStateElement Source { get { return source; } }

        public override float Height => source.Height;

        public override float Radius => source.Radius;

        [SerializeField]
        protected float multiplier = 2f;
        public override float Multiplier => Mathf.Lerp(source.Multiplier, multiplier, Axis);

        public float Axis
        {
            get
            {
                switch (mode)
                {
                    case InputMode.Toggle:
                        return 1f;

                    case InputMode.Hold:
                        return Input.Sprint.Axis;
                }

                throw new NotImplementedException();
            }
        }

        [SerializeField]
        protected InputMode mode = InputMode.Toggle;
        public InputMode Mode { get { return mode; } }
        public enum InputMode
        {
            Toggle, Hold
        }

        public virtual bool CanPerform
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
                if (Active == false && Input.Sprint.Button.Press && CanPerform)
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
                    else if (CanPerform)
                        Begin();
                }
            }

            if (Active && CanPerform == false)
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