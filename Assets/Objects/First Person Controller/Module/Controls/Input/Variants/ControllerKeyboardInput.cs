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
	public class ControllerKeyboardInput : ControllerInput.Context
    {
        protected override void Process()
        {
            base.Process();

            Move = new Vector2()
            {
                x = ControllerInput.GetAxis(KeyCode.D, KeyCode.A),
                y = ControllerInput.GetAxis(KeyCode.W, KeyCode.S)
            };

            Look.SetValue(Input.GetAxis("Mouse Delta X"), Input.GetAxis("Mouse Delta Y"));

            Jump = Input.GetKey(KeyCode.Space);

            Sprint = Input.GetKey(KeyCode.LeftShift) ? 1f : 0f;

            Crouch = Input.GetKey(KeyCode.C);

            Prone = Input.GetKey(KeyCode.LeftControl);

            Lean = ControllerInput.GetAxis(KeyCode.E, KeyCode.Q);
        }
    }
}