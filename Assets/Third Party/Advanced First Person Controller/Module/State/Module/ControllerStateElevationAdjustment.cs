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
	public class ControllerStateElevationAdjustment : ControllerState.Module
	{
        public virtual void Process(ControllerState.IData target)
        {
            if (Controller.IsGrounded)
            {
                var delta = State.Height - target.Height;

                Controller.transform.position -= Controller.transform.up * delta / 2f;
            }
        }
    }
}