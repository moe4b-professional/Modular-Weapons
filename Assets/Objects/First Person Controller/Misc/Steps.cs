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
	public class Steps : MonoBehaviour
	{
        public List<Transform> elements;

        public Vector3 size = new Vector3(0.5f, 0.25f, 0.5f);

        public float width = 4f;

        void OnValidate()
        {
            Rebuild();
        }

        void Rebuild()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].position = transform.position + transform.up * size.y * i;

                elements[i].localScale = new Vector3(width - (i * size.x), size.y, width - (i * size.z));
            }
        }
    }
}