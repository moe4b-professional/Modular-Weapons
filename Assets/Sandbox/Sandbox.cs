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
        void OnDrawGizmos()
        {
            var normal = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;

            Gizmos.DrawLine(transform.position, transform.position + (normal * 2));
        }
    }
}