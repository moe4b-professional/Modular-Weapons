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
	public class PlayerKeyboardInput : PlayerInput
	{
        protected override void Process()
        {
            base.Process();

            Primary = Input.GetKey(KeyCode.Mouse0) ? 1f : 0f;
            Secondary = Input.GetKey(KeyCode.Mouse1) ? 1f : 0f;

            Reload = Input.GetKey(KeyCode.R);

            SwitchWeapon = -Input.mouseScrollDelta.y;
            SwitchWeapon += Input.GetKey(KeyCode.LeftAlt) ? 1f : 0f;

            SwitchActionMode = Input.GetKey(KeyCode.X);
            SwitchSight = Input.GetKey(KeyCode.Mouse2) | Input.GetKey(KeyCode.T);
        }
    }
}