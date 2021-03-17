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

using System.Text;
using System.Reflection;

namespace Game
{
	[Serializable]
	public abstract class MonoScriptSelection
	{
		[SerializeField]
		Object asset = default;
		public Object Asset => asset;

		public Type Type => TypeCache.Load(asset, Argument);

		public abstract Type Argument { get; }

		public static class TypeCache
		{
			public static Dictionary<Object, Type> Dictionary { get; private set; }

			public static Type Load(Object asset, Type argument)
			{
				TryLoad(asset, argument, out var type);

				return type;
			}

			public static bool TryLoad(Object asset, Type argument, out Type type)
			{
				if (asset == null)
				{
					type = null;
					return true;
				}

				if (Dictionary.TryGetValue(asset, out type) == false)
				{
					type = TypeList[asset];

					if (argument.IsAssignableFrom(type) == false) return false;

					Dictionary.Add(asset, type);

					return true;
				}

				return true;
			}

			static TypeCache()
			{
				Dictionary = new Dictionary<Object, Type>();
			}
		}

		public static MonoScriptTypeList TypeList => MonoScriptTypeList.Instance;

#if UNITY_EDITOR
		public static class Bindings
		{
			public static Dictionary<Type, Element> Dictionary { get; private set; }

			public class Element
			{
				public Glossary<int, Type> Glossary { get; protected set; }

				public Type this[int index] => Glossary[index];
				public int this[Type type] => Glossary[type];

				public GUIContent[] Content { get; protected set; }

				public Element(Glossary<int, Type> glossary, GUIContent[] content)
				{
					this.Glossary = glossary;
					this.Content = content;
				}
			}

			public static bool Contains(Type argument) => Dictionary.ContainsKey(argument);

			public static void Register<T>() => Register(typeof(T));
			public static void Register(Type argument)
            {
				var list = new List<Type>();

				for (int i = 0; i < TypeList.Count; i++)
				{
					if (argument.IsAssignableFrom(TypeList[i].Type) == false) continue;

					list.Add(TypeList[i].Type);
				}

				Register(argument, list);
			}
			public static void Register(Type argument, IList<Type> list)
			{
				var glossary = new Glossary<int, Type>(list.Count);

				var content = new GUIContent[list.Count + 1];

				content[0] = new GUIContent("None");

				for (int i = 0; i < list.Count; i++)
				{
					glossary.Add(i, list[i]);

					var label = PrettifyName(list[i].Name);
					content[i + 1] = new GUIContent(label);
				}

				Dictionary[argument] = new Element(glossary, content);
			}

			static Bindings()
			{
				Dictionary = new Dictionary<Type, Element>();
			}
		}

		public static class Arguments
		{
			public static Dictionary<string, Type> Cache { get; private set; }

			public static Type Load(SerializedProperty property)
			{
				if (Cache.TryGetValue(property.propertyPath, out var argument) == false)
					argument = Extract(property);

				return argument;
			}

			static Type Extract(SerializedProperty property)
			{
				var target = property.serializedObject.targetObject;

				var path = property.propertyPath.Replace(".Array.data", "");

				var segments = path.Split('.');

				var type = target.GetType();

				var bindings = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

				for (int i = 0; i < segments.Length; i++)
				{
					var segment = segments[i];

					var isArray = segment.Contains("[") && segment.Contains("]");

					if (isArray)
					{
						var start = segment.IndexOf('[');
						segment = segment.Remove(start);
					}

					var field = type.GetField(segment, bindings);

					type = isArray ? field.FieldType.GetElementType() : field.FieldType;
				}

				return type.BaseType.GenericTypeArguments[0];
			}

			static Arguments()
			{
				Cache = new Dictionary<string, Type>();
			}
		}

		[CustomPropertyDrawer(typeof(MonoScriptSelection), true)]
		public class Drawer : PropertyDrawer
		{
			public SerializedProperty property;

			public Type argument;

			public Bindings.Element binding;

			public SerializedProperty asset;

			void Init(SerializedProperty reference)
			{
				if (property == reference) return;

				property = reference;

				argument = Arguments.Load(property);

				if (Bindings.Contains(argument) == false) Bindings.Register(argument);

				binding = Bindings.Dictionary[argument];

				asset = property.FindPropertyRelative(nameof(MonoScriptSelection.asset));
			}

			public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => EditorGUIUtility.singleLineHeight;

			public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
			{
				Init(property);

				if (TypeCache.TryLoad(asset.objectReferenceValue, argument, out var type))
					DrawPopup(rect, label, type);
				else
					DrawError(rect);
			}

			void DrawError(Rect rect)
			{
				var width = 80f;
				var spacing = 5f;

				rect.width -= width;

				EditorGUI.HelpBox(rect, $"Type '{PrettifyName(asset.objectReferenceValue.name)}' Invalid", MessageType.Error);

				rect.x += rect.width + spacing;
				rect.width = width - spacing;

				if (GUI.Button(rect, "Clear"))
					asset.objectReferenceValue = null;
			}

			void DrawPopup(Rect rect, GUIContent label, Type type)
			{
				var index = type == null ? 0 : binding[type] + 1;

				var selection = EditorGUI.Popup(rect, label, index, binding.Content);

				var changed = (selection != index) || (selection == 0 && asset.objectReferenceValue != null);

				if (changed)
				{
					if (selection == 0)
					{
						asset.objectReferenceValue = null;
					}
					else
					{
						var target = binding[selection - 1];

						asset.objectReferenceValue = TypeList[target];
					}
				}
			}
		}
#endif

		public static string PrettifyName(string value)
		{
			var builder = new StringBuilder();

			for (int i = 0; i < value.Length; i++)
			{
				var current = value[i];

				if (char.IsUpper(current))
				{
					if (i + 1 < value.Length && i > 0)
					{
						var next = value[i + 1];
						var previous = value[i - 1];

						if (char.IsLower(previous))
							builder.Append(" ");
					}
				}

				builder.Append(value[i]);
			}

			return builder.ToString();
		}
	}

	[Serializable]
	public abstract class MonoScriptSelection<T> : MonoScriptSelection
	{
		public override Type Argument => typeof(T);
	}
}