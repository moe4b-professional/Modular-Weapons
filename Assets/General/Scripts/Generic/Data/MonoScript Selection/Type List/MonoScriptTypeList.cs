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
    public class MonoScriptTypeList : ScriptableObject
    {
        public static MonoScriptTypeList Instance { get; protected set; }

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static void OnLoad()
        {
            var list = Resources.LoadAll<MonoScriptTypeList>("");

            if (list.Length == 0)
            {
                Debug.LogError($"No {nameof(MonoScriptTypeList)} Asset Found");
                return;
            }
            else
            {
                Instance = list[0];

                if (list.Length > 1)
                    Debug.LogWarning($"Multiple {nameof(MonoScriptTypeList)}s Found, Please Ensure That Only One Exists");
            }

            Instance.Configure();
        }

        [SerializeField]
        List<Element> list = default;
        public List<Element> List => list;
        [Serializable]
        public class Element
        {
            [SerializeField]
            Object asset = default;
            public Object Asset => asset;

            public Type Type { get; protected set; }
            public void Parse() => Type = Type.GetType(ID);

            [SerializeField]
            string _ID;
            public string ID
            {
                get => _ID;
                set => _ID = value;
            }

            public Element(Object asset, string ID)
            {
                this.asset = asset;
                this._ID = ID;
            }

#if UNITY_EDITOR
            public static Element From(MonoScript script)
            {
                var id = script.GetClass().AssemblyQualifiedName;

                return new Element(script, id);
            }
#endif
        }

        public int Count => list.Count;

        public Element this[int index] => list[index];

        public Glossary<Type, Object> Glossary { get; protected set; }

        public Object this[Type type] => Glossary[type];
        public bool Contains(Type type) => Glossary.Contains(type);

        public Type this[Object script] => Glossary[script];
        public bool Contains(Object script) => Glossary.Contains(script);

        void Configure()
        {
#if UNITY_EDITOR
            Refresh();
#endif

            Register();

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }

#if UNITY_EDITOR
        void Refresh()
        {
            list = new List<Element>(500);

            foreach (var script in Iterate())
            {
                var type = script.GetClass();

                if (type == null) continue;

                var element = Element.From(script);

                list.Add(element);
            }
        }

        IEnumerable<MonoScript> Iterate()
        {
            var guids = AssetDatabase.FindAssets("t:MonoScript");

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);

                var asset = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

                yield return asset;
            }
        }
#endif

        public delegate void RegisterDelegate();
        public static event RegisterDelegate OnRegister;
        void Register()
        {
            Glossary = new Glossary<Type, Object>(list.Capacity);

            for (int i = 0; i < list.Count; i++)
            {
                list[i].Parse();

                if (list[i].Type == null) continue;

                Glossary.Add(list[i].Type, list[i].Asset);
            }

            OnRegister?.Invoke();
        }
    }
}