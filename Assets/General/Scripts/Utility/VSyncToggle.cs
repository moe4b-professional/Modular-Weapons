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
	public class VSyncToggle : MonoBehaviour
	{
        public KeyCode key = KeyCode.V;

        bool isOn = true;

        void Start()
        {
            UpdateState();
        }

        void Update()
        {
            if (Input.GetKeyDown(key))
                Toggle();
        }

        void Toggle()
        {
            isOn = !isOn;

            UpdateState();
        }

        void UpdateState()
        {
            QualitySettings.vSyncCount = isOn ? 1 : 0;
        }
    }
}