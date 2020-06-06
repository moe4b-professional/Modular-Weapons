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
	public class CursorControls : MonoBehaviour
	{
        public KeyCode key = KeyCode.F;

        bool isOn = false;

        private void Start()
        {
            UpdateState();
        }

        private void Update()
        {
            if (Input.GetKeyDown(key))
                Toggle();
        }

        private void Toggle()
        {
            isOn = !isOn;

            UpdateState();
        }

        private void UpdateState()
        {
            Cursor.visible = isOn;

            Cursor.lockState = isOn ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}