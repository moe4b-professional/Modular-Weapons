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
    [RequireComponent(typeof(Camera))]
	public class ManualCameraRender : MonoBehaviour
	{
        [SerializeField]
        protected int _FPS = 60;
        public int FPS { get { return _FPS; } }

        public float FrameTime => 1f / FPS;

        float timer = 0f;

        Camera component;

        void Awake()
        {
            component = GetComponent<Camera>();

            component.enabled = false;
        }

        void Update()
        {
            timer += Time.unscaledDeltaTime;

            if (timer >= FrameTime)
            {
                timer = 0f;
                component.Render();
            }
        }
    }
}