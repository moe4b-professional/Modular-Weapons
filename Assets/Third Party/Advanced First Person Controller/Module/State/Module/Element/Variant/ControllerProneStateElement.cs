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
	public class ControllerProneStateElement : ControllerStateElement, ControllerJumpConstraint.IInterface
    {
        bool ControllerJumpConstraint.IInterface.Active => Weight > 0f;

        public override void Init()
        {
            base.Init();

            Controller.Jump.Constraint.Register(this);
        }

        protected override void Process()
        {
            base.Process();

            if (Input.Prone.Press)
                Toggle(Sets.Normal);

            if (Input.Jump.Press && Active)
                State.Transition.Set(Sets.Normal);
        }
    }
}