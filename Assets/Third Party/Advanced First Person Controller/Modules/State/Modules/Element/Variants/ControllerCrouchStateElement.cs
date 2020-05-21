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
	public class ControllerCrouchStateElement : ControllerStateElement
    {
        protected override void Process()
        {
            base.Process();

            if (Input.Crouch.Press)
                Toggle(Sets.Normal);

            if (Input.Jump.Press && Active)
                State.Transition.Set(Sets.Normal);
        }
    }
}