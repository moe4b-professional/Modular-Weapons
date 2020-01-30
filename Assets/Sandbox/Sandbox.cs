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
	public class Sandbox : MonoBehaviour
	{
        ButtonInput button;

        private void Start()
        {
            button = new ButtonInput();
        }

        private void Update()
        {
            button.Process(Input.GetKey(KeyCode.Mouse0));

            if (button.Press) Debug.Log("Button Down");

            if (button.Up) Debug.Log("Button Up");
        }
    }
}