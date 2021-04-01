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
	public static class PersonalTools
	{
        public static void SetLayer(GameObject target, int layerIndex) => SetLayer(target.transform, layerIndex);
		public static void SetLayer(Transform target, int layerIndex)
        {
            target.gameObject.layer = layerIndex;

            for (int i = 0; i < target.childCount; i++)
                SetLayer(target.GetChild(i), layerIndex);
        }

        public static Bounds CalculateBounds(GameObject gameObject)
        {
            var renderers = gameObject.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
                return new Bounds(gameObject.transform.position, Vector3.zero);

            var bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);

            return bounds;
        }
    }
}