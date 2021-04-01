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
    public class ReadOnly : PropertyAttribute
    {
#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ReadOnly))]
        public class Drawer : PropertyDrawer
        {
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return EditorGUI.GetPropertyHeight(property, label, true);
            }

            public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
            {
                GUI.enabled = false;

                EditorGUI.PropertyField(rect, property, label, true);

                GUI.enabled = true;
            }
        }
#endif
    }
}