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
        [Range(0f, 1f)]
        protected float cameraAlignment = 0.5f;
        public float CameraAlignment { get { return cameraAlignment; } }

        public Vector3 Axis => Vector3.forward;

        public ControllerRig.CameraData Rig => Controller.Rig.camera;

        public Quaternion Offset { get; protected set; }
        public Quaternion AlignmentOffset { get; protected set; }

        public ControllerInput.LeanInput Input => Controller.Input.Lean;

        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            CalculateOffset();
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            QuatTool.Subtract(Rig.Pivot, Offset);
            QuatTool.Subtract(Rig.Anchor, AlignmentOffset);

            CalculateTarget();

            Rate = Mathf.MoveTowards(Rate, Target, speed * Time.deltaTime);

            CalculateOffset();

            QuatTool.Add(Rig.Pivot, Offset);
            QuatTool.Add(Rig.Anchor, AlignmentOffset);
        }

        protected virtual void CalculateTarget()
        {
            if (mode == InputMode.Hold)
            {
                Target = Input.Axis;
            }
            if (mode == InputMode.Toggle)
            {
                if (Input.Right.Press) ToggleTarget(1f);

                if (Input.Left.Press) ToggleTarget(-1f);
            }
        }
        protected virtual void ToggleTarget(float value)
        {
            if (Target == value)
                Target = 0f;
            else
                Target = value;
        }

        protected virtual void CalculateOffset()
        {
            Offset = Quaternion.Euler(Axis * -Angle);

            AlignmentOffset = Quaternion.Lerp(Quaternion.identity, Quaternion.Inverse(Offset), cameraAlignment);
        }
    }
}