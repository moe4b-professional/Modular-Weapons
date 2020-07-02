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
    public class AutoRenderTexture : MonoBehaviour
	{
		[SerializeField]
        protected Camera source;

        [SerializeField]
        protected Renderer target;

        [SerializeField]
        protected int width = 512;
        public int Width { get { return width; } }

        [SerializeField]
        protected int height = 512;
        public int Height { get { return height; } }

        public RenderTexture Texture { get; protected set; }

        void Start()
        {
            Texture = Create();

            source.targetTexture = Texture;

            target.material.mainTexture = Texture;
        }

        RenderTexture Create()
        {
            var result = new RenderTexture(width, height, 16);

            return result;
        }

        void OnDestroy()
        {
            if (Texture != null && Texture.IsCreated())
                Texture.Release();
        }
    }
}