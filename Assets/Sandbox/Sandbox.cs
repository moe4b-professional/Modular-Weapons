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

using System.Reflection;

using MB;
using UnityEngine.Serialization;

namespace Game
{
    public class Sandbox : MonoBehaviour
    {
        //[field: SerializeField]
        //public float Number { get; private set; } = 10;

        [SerializeField]
        [FormerlySerializedAs("<Number>k__BackingField")]
        float number;
        public float Number => number;

        void Start()
        {

        }

        static string GetAutoPropertyBackingFieldName(string name) => $"<{name}>k__BackingField";
    }
}