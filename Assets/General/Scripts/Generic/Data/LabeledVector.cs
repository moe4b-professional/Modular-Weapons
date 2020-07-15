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
    public static class VectorLabels
    {
        public const string Seperator = "`";

        public const string Tilt = "Tilt";
        public const string Pan = "Pan";
        public const string Roll = "Roll";
        public const string Rotation = Tilt + Seperator + Pan + Seperator + Roll;

        public const string Horizontal = "Horizontal";
        public const string Vertical = "Vertical";
        public const string Fordical = "Fordical";
        public const string Position = Horizontal + Seperator + Vertical + Seperator + Fordical;
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public sealed class LabeledVectorAttribute : PropertyAttribute
    {
        public string[] Names { get; private set; }

        public string Read(int index)
        {
            if (index < 0) return null;

            if (index > Names.Length - 1) return null;

            return Names[index];
        }

        public LabeledVectorAttribute(params string[] names)
        {
            if(names.Length > 0 && names[0].Contains(VectorLabels.Seperator))
                this.Names = names[0].Split(VectorLabels.Seperator[0]);
            else
                this.Names = names;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(LabeledVectorAttribute))]
    public class LabeledVectorDrawer : PropertyDrawer
    {
        SerializedProperty property;

        SerializedProperty x;
        SerializedProperty y;
        SerializedProperty z;
        SerializedProperty w;

        IEnumerable<SerializedProperty> IterateComponents()
        {
            yield return x;

            yield return y;

            yield return z;

            yield return w;
        }

        IEnumerable<SerializedProperty> IterateActiveComponents()
        {
            foreach (var component in IterateComponents())
            {
                if (component != null)
                    yield return component;
            }
        }

        public int Count
        {
            get
            {
                var value = 0;

                foreach (var property in IterateActiveComponents())
                    value++;

                return value;
            }
        }

        new LabeledVectorAttribute attribute;

        public const float TitleSpacing = 20f;
        public const float ComponentSpacing = 5f;
        public const float ComponentLabelSpacing = 5f;

        public int IndentLevel { get; protected set; }

        protected virtual void Init(SerializedProperty target)
        {
            property = target;

            x = property.FindPropertyRelative(nameof(x));
            y = property.FindPropertyRelative(nameof(y));
            z = property.FindPropertyRelative(nameof(z));
            w = property.FindPropertyRelative(nameof(w));

            attribute = base.attribute as LabeledVectorAttribute;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init(property);

            var height = EditorGUIUtility.singleLineHeight;

            return height;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            rect = EditorGUI.IndentedRect(rect);

            IndentLevel = EditorGUI.indentLevel;

            EditorGUI.indentLevel = 0;
            {
                Init(property);

                DrawProperty(ref rect, property, label);

                DrawComponents(ref rect);
            }
            EditorGUI.indentLevel = IndentLevel;
        }

        protected virtual void DrawProperty(ref Rect rect, SerializedProperty property, GUIContent label)
        {
            var width = GUI.skin.label.CalcSize(label).x;

            var size = new Vector2(width, EditorGUIUtility.singleLineHeight);

            var area = new Rect(rect.position, size);

            EditorGUI.LabelField(area, label);

            rect.x += size.x + TitleSpacing;
            rect.width -= size.x + TitleSpacing;
        }

        protected virtual void DrawComponents(ref Rect rect)
        {
            var componentWidth = (rect.width / Count) - ComponentSpacing;

            var index = 0;
            foreach (var component in IterateActiveComponents())
            {
                DrawComponent(ref rect, component, index, componentWidth);

                rect.x += ComponentSpacing;
                rect.width -= ComponentSpacing;

                index++;
            }
        }
        protected virtual void DrawComponent(ref Rect rect, SerializedProperty component, int index, float width)
        {
            var size = new Vector2(width, EditorGUIUtility.singleLineHeight);

            var area = new Rect(rect.position, size);

            var text = ComponentToLabel(component, index);

            var label = new GUIContent(text);

            var originalLabelWidth = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = GUI.skin.label.CalcSize(label).x + ComponentLabelSpacing;

            EditorGUI.PropertyField(area, component, label);

            rect.x += size.x;
            rect.width -= size.x;

            EditorGUIUtility.labelWidth = originalLabelWidth;
        }

        protected virtual string ComponentToLabel(SerializedProperty component, int index)
        {
            var result = attribute.Read(index);

            if (string.IsNullOrEmpty(result))
                result = component.displayName;

            return result;
        }
    }
#endif
}