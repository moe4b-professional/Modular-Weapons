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
	public class WeaponAmmoUsesProcedureChange : WeaponAmmo.Module
	{
        [SerializeField]
        protected TargetData[] targets;
        public TargetData[] Targets { get { return targets; } }
        [Serializable]
        public class TargetData
        {
            [SerializeField]
            protected string name;
            public string Name { get { return name; } }

            [SerializeField]
            protected GameObject[] gameObjects;
            public GameObject[] GameObjects { get { return gameObjects; } }

            [SerializeField]
            [ConditionDrawer("Uses", true)]
            protected IntegerCondition condition;
            public IntegerCondition Condition { get { return condition; } }

            public virtual void UpdateState(int remainingUses)
            {
                var active = condition.Evaluate(remainingUses);

                for (int i = 0; i < gameObjects.Length; i++)
                    gameObjects[i].SetActive(active);
            }
        }
        
        public override void Init()
        {
            base.Init();

            Ammo.OnRefill += RefillCallback;
            Ammo.OnConsumption += ConsumptionCallback;

            UpdateState();
        }

        protected virtual void UpdateState()
        {
            for (int i = 0; i < targets.Length; i++)
                targets[i].UpdateState(Ammo.RemainingUses);
        }

        void RefillCallback() => UpdateState();
        void ConsumptionCallback() => UpdateState();
    }

    [Serializable]
    public class IntegerCondition
    {
        [SerializeField]
        protected OperatorMode operation;
        public OperatorMode Operation { get { return operation; } }
        [Serializable]
        public enum OperatorMode
        {
            Equal, Less, More
        }

        [SerializeField]
        protected int value;
        public int Value { get { return value; } }

        public virtual bool Evaluate(int target)
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
                var text = Plural ? "are" : "is";

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
                        return "to";

                    case OperatorMode.Less:
                    case OperatorMode.More:
                        return "than";
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