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
    [Serializable]
    public abstract class ReferenceCollection
    {
        public enum Scope
        {
            All, Local
        }

        public interface IElement
        {
            Transform transform { get; }
        }
    }

    [Serializable]
    public class ReferencedCollection<TReference, TInstance> : ReferenceCollection
            where TReference : Component
            where TInstance : class, ReferenceCollection.IElement
    {
        public TReference Reference { get; protected set; }

        public List<TInstance> List { get; protected set; }

        public virtual void ForAll(Action<TInstance> action)
        {
            for (int i = 0; i < List.Count; i++)
                action(List[i]);
        }
        public virtual void ForAll<TType>(Action<TType> action)
            where TType : class
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i] is TType)
                    action(List[i] as TType);
        }

        public virtual void Register(TInstance instance)
        {
            List.Add(instance);
        }

        public virtual void Register(IList<TInstance> list) => Register(list, Scope.Local);
        public virtual void Register(IList<TInstance> list, Scope scope)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (CheckScope(list[i], scope))
                    Register(list[i]);
            }
        }

        public virtual void Register(GameObject root)
        {
            var list = Dependancy.GetAll<TInstance>(root);

            Register(list);
        }

        public virtual void Register<TCReference, TCInstance>(ReferencedCollection<TCReference, TCInstance> collection, Scope scope)
            where TCReference : Component
            where TCInstance : class, IElement
        {
            var list = collection.FindAll<TInstance>();

            Register(list, scope);
        }
        public virtual void Register<TCReference, TCInstance>(ReferencedCollection<TCReference, TCInstance> collection)
            where TCReference : Component
            where TCInstance : class, IElement
        {
            Register(collection, Scope.Local);
        }

        protected virtual bool CheckScope(TInstance instance, Scope scope)
        {
            if (scope == Scope.All) return true;

            if(scope == Scope.Local)
            {
                if (instance.transform == Reference.transform) return true;

                return instance.transform.IsChildOf(Reference.transform);
            }

            throw new NotImplementedException();
        }
        
        public virtual TType Find<TType>()
            where TType : class
        {
            for (int i = 0; i < List.Count; i++)
                if (List[i] is TType)
                    return List[i] as TType;

            return null;
        }
        public virtual List<TType> FindAll<TType>()
            where TType : class
        {
            var result = new List<TType>();

            for (int i = 0; i < List.Count; i++)
                if (List[i] is TType)
                    result.Add(List[i] as TType);

            return result;
        }

        public virtual TType Depend<TType>()
            where TType : class
        {
            var target = Find<TType>();

            if (target == null)
            {
                var exception = Dependancy.CreateException<TType>(Reference);

                throw exception;
            }

            return target;
        }

        public ReferencedCollection(TReference reference)
        {
            this.Reference = reference;

            this.List = new List<TInstance>();
        }
    }
}