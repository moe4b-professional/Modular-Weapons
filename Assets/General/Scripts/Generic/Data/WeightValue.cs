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
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class WeightValueAttribute : PropertyAttribute
    {
        public string Left { get; private set; }

        public string Right { get; private set; }

        public WeightValueAttribute(string left, string right)
        {
            this.Left = left;

            this.Right = right;
        }
    }

    [Serializable]
    public struct WeightValue
    {
        [SerializeField]
        float value;
        public float Value => value;

        public float Left => Mathf.Lerp(1f, 0f, value);
        public float Right => value;

        public WeightValue(float value)
        {
            this.value = value;
        }

        public static implicit operator WeightValue(float value) => new WeightValue(value);
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(WeightValueAttribute))]
    public class WeightValueDrawer : PropertyDrawer
    {
        SerializedProperty value;

        new WeightValueAttribute attribute;

        public const float Spacing = 5f;

        public int indentLevel;

        public const float IndentWidth = 15f;

        protected virtual void Init(SerializedProperty property)
        {
            value = property.FindPropertyRelative(nameof(value));

            attribute = base.attribute as WeightValueAttribute;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            Init(property);

            rect = EditorGUI.IndentedRect(rect);

            indentLevel = EditorGUI.indentLevel;

            EditorGUI.indentLevel = 0;
            {
                DrawLabel(ref rect, property, label);

                DrawSide(ref rect, attribute.Left);

                SpaceUp(ref rect, Spacing);

                DrawSlider(ref rect, CalculateRightSidePadding() + Spacing);

                SpaceUp(ref rect, Spacing);

                DrawSide(ref rect, attribute.Right);
            }
            EditorGUI.indentLevel = indentLevel;
        }

        protected virtual void DrawLabel(ref Rect rect, SerializedProperty property, GUIContent label)
        {
            var width = GUI.skin.label.CalcSize(label).x + 5f;

            var size = new Vector2(width, EditorGUIUtility.singleLineHeight);

            var area = new Rect(rect.position, size);

            property.isExpanded = EditorGUI.Foldout(area, property.isExpanded, label);

            SpaceUp(ref rect, size.x);

            var space = EditorGUIUtility.labelWidth - size.x - (indentLevel * IndentWidth);

            if (property.isExpanded)
            {
                DrawField(ref rect, ref space);
            }
            
            SpaceUp(ref rect, space);
        }

        protected virtual void DrawField(ref Rect rect, ref float spacing)
        {
            var width = 60f;

            var size = new Vector2(width, EditorGUIUtility.singleLineHeight);

            var area = new Rect(rect.position, size);

            value.floatValue = EditorGUI.FloatField(area, value.floatValue);

            SpaceUp(ref rect, size.x);

            spacing -= width;
        }

        protected virtual void DrawSide(ref Rect rect, string text)
        {
            var content = new GUIContent(text);

            var width = GUI.skin.label.CalcSize(content).x;

            var size = new Vector2(width, EditorGUIUtility.singleLineHeight);

            var area = new Rect(rect.position, size);

            EditorGUI.LabelField(area, content);

            SpaceUp(ref rect, size.x);
        }

        protected virtual void DrawSlider(ref Rect rect, float padding)
        {
            var size = new Vector2(rect.width - padding, EditorGUIUtility.singleLineHeight);

            var area = new Rect(rect.position, size);

            value.floatValue = GUI.HorizontalSlider(area, value.floatValue, 0f, 1f);

            SpaceUp(ref rect, size.x);
        }

        protected virtual float CalculateRightSidePadding()
        {
            var content = new GUIContent(attribute.Right);

            var size = GUI.skin.label.CalcSize(content);

            return size.x;
        }

        protected virtual void SpaceUp(ref Rect rect, float space)
        {
            rect.x += space;
            rect.width -= space;
        }
    }
#endif
}