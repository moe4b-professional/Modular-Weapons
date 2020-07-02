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
	public static class Dependancy
	{
        public static TComponent Get<TComponent>(GameObject target) where TComponent : class => Get<TComponent>(target, Scope.Hierarchy);
        public static TComponent Get<TComponent>(GameObject target, Scope scope)
            where TComponent : class
        {
            TComponent component;

            if(scope == Scope.Childern)
                component = null;
            else
                component = target.GetComponent<TComponent>();

            if (IsNull(component))
            {
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    var child = target.transform.GetChild(i);

                    component = Get<TComponent>(child.gameObject, Scope.Hierarchy);

                    if (!IsNull(component)) break;
                }
            }

            return component;
        }
        
        public static List<TComponent> GetAll<TComponent>(GameObject target) where TComponent : class => GetAll<TComponent>(target, Scope.Hierarchy);
        public static List<TComponent> GetAll<TComponent>(GameObject target, Scope scope)
            where TComponent : class
        {
            var list = new List<TComponent>();

            if(scope == Scope.Local || scope == Scope.Hierarchy)
            {
                var components = target.GetComponents<TComponent>();

                list.AddRange(components);
            }

            if (scope == Scope.Childern || scope == Scope.Hierarchy)
            {
                for (int i = 0; i < target.transform.childCount; i++)
                {
                    var child = target.transform.GetChild(i);

                    var components = GetAll<TComponent>(child.gameObject, Scope.Hierarchy);

                    list.AddRange(components);
                }
            }

            return list;
        }

        public enum Scope
        {
            /// <summary>
            /// Current GameObject
            /// </summary>
            Local,

            /// <summary>
            /// Current GameObject And it's Childern
            /// </summary>
            Hierarchy,

            /// <summary>
            /// Childern of Current GameObject
            /// </summary>
            Childern
        }

        public static Exception CreateException<TDependancy>(object dependant)
        {
            var text = FormatException<TDependancy>(dependant);

            return new Exception(text);
        }
        public static string FormatException<TDependancy>(object dependant)
        {
            var component = dependant as Component;

            var text = dependant.GetType().Name + " Requires a dependancy of type " + typeof(TDependancy).Name;

            if (component != null)
                text += " on gameObject " + component.gameObject.name;

            return text;
        }

        public static bool IsNull(object target)
        {
            if (target == null) return true;

            if (target.Equals(null)) return true;

            return false;
        }
    }
}