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
	public class Surface : MonoBehaviour
	{
        [SerializeField]
        protected SurfaceMaterial material;
        public SurfaceMaterial Material { get { return material; } }

        public static Surface Get(Collider collider)
        {
            Surface surface;

            surface = collider.GetComponent<Surface>();
            if (surface != null) return surface;

            if (collider.attachedRigidbody == null) return null;

            surface = collider.attachedRigidbody.GetComponent<Surface>();
            if (surface != null) return surface;

            return null;
        }
    }
}