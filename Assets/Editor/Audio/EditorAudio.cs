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
    [CreateAssetMenu]
	public class EditorAudio : ScriptableObject
	{
		[SerializeField]
        protected float volume;
        public float Volume
        {
            get => volume;
            set
            {
                if(volume != value) EditorUtility.SetDirty(this);

                volume = value;

                if (Application.isPlaying) UpdateState();
            }
        }

        public static EditorAudio Instance { get; protected set; }

        [RuntimeInitializeOnLoadMethod]
        static void OnLoad()
        {
            Instance = Find();

            if (Instance == null)
                Debug.LogWarning("No " + nameof(EditorAudio) + " Asset Found");
            else
                Instance.Configure();
        }

        private void Configure()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            AudioListener.volume = Volume;
        }

        static EditorAudio Find()
        {
            var assets = AssetDatabase.FindAssets("t:" + nameof(EditorAudio));

            if (assets == null || assets.Length == 0) return null;

            var path = AssetDatabase.GUIDToAssetPath(assets[0]);

            return AssetDatabase.LoadAssetAtPath<EditorAudio>(path);
        }

        [CustomEditor(typeof(EditorAudio))]
        public class Inspector : Editor
        {
            new EditorAudio target;

            private void OnEnable()
            {
                target = base.target as EditorAudio;
            }

            public override void OnInspectorGUI()
            {
                target.Volume = EditorGUILayout.Slider("Volume", target.Volume, 0f, 1f);
            }
        }
    }
}