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
        public static TComponent Get<TComponent>(GameObject target)
            where TComponent : class
        {
            return Get<TComponent>(target, Scope.CurrentToChildern);
        }
        public static TComponent Get<TComponent>(GameObject target, Scope scope)
            where TComponent : class
        {
            TComponent component;

            if(scope == Scope.Childern)
            {
                component = null;

                scope = Scope.CurrentToChildern;
            }
            else if(scope == Scope.Parents)
            {
                component = null;

                scope = Scope.CurrentToParents;
            }
            else
            {
                component = target.GetComponent<TComponent>();
            }

            if (IsNull(component))
            {
                if (scope == Scope.CurrentToChildern)
                {
                    for (int i = 0; i < target.transform.childCount; i++)
                    {
                        component = Get<TComponent>(target.transform.GetChild(i).gameObject, scope);

                        if (!IsNull(component)) break;
                    }
                }

                if (scope == Scope.CurrentToParents && target.transform.parent != null)
                    component = Get<TComponent>(target.transform.parent.gameObject, scope);
            }

            return component;
        }
        
        public static List<TComponent> GetAll<TComponent>(GameObject target)
            where TComponent : class
        {
            return GetAll<TComponent>(target, Scope.CurrentToChildern);
        }
		public static List<TComponent> GetAll<TComponent>(GameObject target, Scope scope)
            where TComponent : class
        {
            var list = new List<TComponent>();

            list.AddRange(target.GetComponents<TComponent>());

            if (scope == Scope.CurrentToChildern)
                for (int i = 0; i < target.transform.childCount; i++)
                    list.AddRange(GetAll<TComponent>(target.transform.GetChild(i).gameObject, scope));

            if (scope == Scope.CurrentToParents)
                if (target.transform.parent != null)
                    list.AddRange(GetAll<TComponent>(target.transform.parent.gameObject, scope));

            return list;
        }

        public enum Scope
        {
            Local, Childern, CurrentToChildern, CurrentToParents, Parents
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