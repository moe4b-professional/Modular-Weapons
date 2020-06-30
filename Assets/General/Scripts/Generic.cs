﻿using System;
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
    public class Modifier
    {
        public class Constraint : Base<Constraint.IInterface>
        {
            public bool Active
            {
                get
                {
                    for (int i = 0; i < List.Count; i++)
                        if (List[i].Active)
                            return true;

                    return false;
                }
            }

            public interface IInterface
            {
                bool Active { get; }
            }
        }

        public class Average : Base<Average.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 0f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Value;

                    return result / List.Count;
                }
            }
        }

        public class Additive : Base<Additive.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 0f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Value;

                    return result;
                }
            }
        }

        public class Scale : Base<Scale.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 1f;

                    var result = 1f;

                    for (int i = 0; i < List.Count; i++)
                        result *= List[i].Value;

                    return result;
                }
            }
        }

        public class Base<TInterface>
            where TInterface : class
        {
            public List<TInterface> List { get; protected set; }

            public virtual void Register(TInterface modifier)
            {
                if (List.Contains(modifier))
                {
                    Debug.LogWarning("Modifier Already Added");
                    return;
                }

                List.Add(modifier);
            }

            public Base()
            {
                List = new List<TInterface>();
            }
        }
    }

    [Serializable]
    public struct ValueRange
    {
        [SerializeField]
        float min;
        public float Min { get { return min; } }

        [SerializeField]
        float max;
        public float Max { get { return max; } }

        public float Random => UnityEngine.Random.Range(min, max);

        public float Lerp(float t) => Mathf.Lerp(min, max, t);

        public float Clamp(float value) => Mathf.Clamp(value, min, max);

        public ValueRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [Serializable]
    public class ForceData
    {
        [SerializeField]
        protected float value;
        public float Value { get { return value; } }

        [SerializeField]
        protected ForceMode mode;
        public ForceMode Mode { get { return mode; } }

        public ForceData(float value, ForceMode mode)
        {
            this.value = value;

            this.mode = mode;
        }
    }

    [Serializable]
    public abstract class Condition
    {
        [SerializeField]
        protected OperatorMode operation = OperatorMode.Equal;
        public OperatorMode Operation { get { return operation; } }
        [Serializable]
        public enum OperatorMode
        {
            Equal, Less, More
        }
    }

    [Serializable]
    public abstract class Condition<TValue> : Condition
    {
        [SerializeField]
        protected TValue value;
        public TValue Value { get { return value; } }

        public abstract bool Evaluate(TValue target);

        public static bool EvaluateAll(IList<Condition<TValue>> conditions, TValue target)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                var result = conditions[i].Evaluate(target);

                if (result == false) return false;
            }

            return true;
        }
    }

    [Serializable]
    public class IntegerCondition : Condition<int>
    {
        public override bool Evaluate(int target)
        {
            switch (operation)
            {
                case OperatorMode.Equal:
                    return target == value;
                case OperatorMode.Less:
                    return target < value;
                case OperatorMode.More:
                    return target > value;
            }

            throw new NotImplementedException();
        }

#if UNITY_EDITOR
        [CustomPropertyDrawer(typeof(ConditionDrawerAttribute))]
        public class Drawer : PropertyDrawer
        {
            SerializedProperty operation;

            SerializedProperty value;

            new ConditionDrawerAttribute attribute;

            public string Name => attribute == null ? "Value" : attribute.Name;

            public bool Plural => attribute == null ? false : attribute.Plural;

            public const float Spacing = 2f;

            protected virtual void Init(SerializedProperty property)
            {
                operation = property.FindPropertyRelative(nameof(operation));

                value = property.FindPropertyRelative(nameof(value));

                attribute = base.attribute as ConditionDrawerAttribute;
            }

            public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

            public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
            {
                Init(property);

                rect = EditorGUI.IndentedRect(rect);

                var indentLevel = EditorGUI.indentLevel;

                EditorGUI.indentLevel = 0;
                {
                    DrawLabel(ref rect);

                    DrawPrefix(ref rect);

                    DrawOperation(ref rect);

                    DrawSuffix(ref rect);

                    DrawValue(ref rect);
                }
                EditorGUI.indentLevel = indentLevel;
            }

            protected virtual void DrawLabel(ref Rect rect)
            {
                var content = new GUIContent(Name);

                var size = GUI.skin.label.CalcSize(content);

                var area = new Rect(rect.position, size);

                EditorGUI.LabelField(area, content);

                rect.x += size.x;
            }

            protected virtual void DrawPrefix(ref Rect rect)
            {
                var text = Plural ? "Are" : "Is";

                var content = new GUIContent(text);

                var size = GUI.skin.label.CalcSize(content);

                var area = new Rect(rect.position, size);

                EditorGUI.LabelField(area, content);

                rect.x += size.x;
            }

            protected virtual void DrawOperation(ref Rect rect)
            {
                var size = new Vector2(60, rect.height);

                var area = new Rect(rect.position, size);

                EditorGUI.PropertyField(area, operation, GUIContent.none, true);

                rect.x += size.x;
            }

            protected virtual void DrawSuffix(ref Rect rect)
            {
                rect.x += Spacing;

                var text = OperatorToSuffix((OperatorMode)operation.enumValueIndex);

                var content = new GUIContent(text);

                var size = GUI.skin.label.CalcSize(content);

                var area = new Rect(rect.position, size);

                EditorGUI.LabelField(area, content);

                rect.x += size.x;
            }

            protected virtual void DrawValue(ref Rect rect)
            {
                var size = new Vector2(40, rect.height);

                var area = new Rect(rect.position, size);

                EditorGUI.PropertyField(area, value, GUIContent.none, true);
            }

            public static string OperatorToSuffix(OperatorMode value)
            {
                switch (value)
                {
                    case OperatorMode.Equal:
                        return "To";

                    case OperatorMode.Less:
                    case OperatorMode.More:
                        return "Than";
                }

                throw new NotImplementedException();
            }
        }
#endif
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true)]
    public sealed class ConditionDrawerAttribute : PropertyAttribute
    {
        public string Name { get; private set; }

        public bool Plural { get; private set; }

        public ConditionDrawerAttribute(string name, bool plural)
        {
            this.Name = name;

            this.Plural = plural;
        }
    }
}