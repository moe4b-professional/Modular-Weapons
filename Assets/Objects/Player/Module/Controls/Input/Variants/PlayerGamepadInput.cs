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
	public class PlayerGamepadInput : PlayerInput
	{
        protected override void Process()
        {
            base.Process();

            Primary = Input.GetAxis("Gamepad Right Trigger");
            Secondary = Input.GetAxis("Gamepad Left Trigger");

            Reload = Input.GetKey(KeyCode.JoystickButton2);

            SwitchWeapon = Input.GetKey(KeyCode.JoystickButton3) ? 1f : 0f;
            SwitchActionMode = Input.GetAxis("Gamepad POV Y") < 0f;
            SwitchSight = Input.GetKey(KeyCode.JoystickButton9);
        }
    }
}