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
        public Vector3 vector;

        void Start()
        {

        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var transform = this.transform.TransformVector(vector);
            Gizmos.DrawSphere(transform, 0.2f);

            Gizmos.color = Color.red;
            var project = Vector3.ProjectOnPlane(vector, this.transform.forward);
            Gizmos.DrawSphere(project, 0.2f);
        }
    }

    public class MyComponent : MonoBehaviour
    {
        public int id;

        void Start()
        {
            MyComponent[] list = FindObjectsOfType<MyComponent>();

            for (int i = 0; i < list.Length; i++)
                list[i].id = i;
        }
    }
}