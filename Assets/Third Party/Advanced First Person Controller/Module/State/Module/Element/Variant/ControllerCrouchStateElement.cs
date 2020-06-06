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
    public class ControllerCrouchStateElement : ControllerStateElement, ControllerJumpConstraint.IInterface
    {
        [SerializeField]
        protected JumpInputAction jumpAction = JumpInputAction.StandUp;
        public JumpInputAction JumpAction { get { return jumpAction; } }
        public enum JumpInputAction
        {
            Jump, StandUp
        }

        bool ControllerJumpConstraint.IInterface.Active => jumpAction == JumpInputAction.StandUp && Weight > 0f;

        public override void Init()
        {
            base.Init();

            Controller.Jump.Constraint.Register(this);
        }

        protected override void Process()
        {
            base.Process();

            if (Input.Crouch.Press)
                Toggle(Sets.Normal);

            if (jumpAction == JumpInputAction.StandUp && Input.Jump.Press && Active)
                State.Transition.Set(Sets.Normal);
        }
    }
}