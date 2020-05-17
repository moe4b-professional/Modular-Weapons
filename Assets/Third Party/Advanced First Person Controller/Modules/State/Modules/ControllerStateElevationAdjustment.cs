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
        public ControllerFeetPosition FeetPosition => Controller.FeetPosition;

        public override void Configure(ControllerState reference)
        {
            base.Configure(reference);
        }

        public virtual void Process(ControllerState.Data target)
        {
            if (Controller.GroundCheck.IsGrounded || true)
            {
                var delta = FeetPosition.Calculate(State) - FeetPosition.Calculate(target);

                var dot = Vector3.Dot(delta, Controller.transform.up);

                Controller.transform.position += Controller.transform.up * dot;
            }
        }
    }
}