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
    public static class References
    {
        public static void Configure<TReference>(TReference reference)
            where TReference : Component
        {
            Configure(reference, reference.gameObject);
        }
        public static void Configure<TReference>(TReference reference, GameObject root)
            where TReference : Component
        {
            var targets = Dependancy.GetAll<IReference<TReference>>(root);

            Configure(reference, targets);
        }
        public static void Configure<TReference>(TReference reference, IList<IReference<TReference>> targets)
        {
            for (int i = 0; i < targets.Count; i++)
                Configure(reference, targets[i]);
        }
        public static void Configure<TReference>(TReference reference, IReference<TReference> target)
        {
            target.Configure(reference);
        }

        public static void Init<TReference>(TReference reference)
            where TReference : Component
        {
            Init(reference, reference.gameObject);
        }
        public static void Init<TReference>(TReference reference, GameObject root)
            where TReference : Component
        {
            var targets = Dependancy.GetAll<IReference<TReference>>(root);

            Init(reference, targets);
        }
        public static void Init<TReference>(TReference reference, IList<IReference<TReference>> targets)
        {
            for (int i = 0; i < targets.Count; i++)
                Init(reference, targets[i]);
        }
        public static void Init<TReference>(TReference reference, IReference<TReference> target)
        {
            target.Init();
        }

        public abstract class BaseCollection<TType>
            where TType : class
        {
            public List<TType> List { get; protected set; }

            public virtual void ForAll<TConstraint>(Action<TConstraint> action)
                where TConstraint : class
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i] is TConstraint)
                        action(List[i] as TConstraint);
            }

            public virtual void Add(TType instance)
            {
                List.Add(instance);
            }

            public virtual T Find<T>()
                where T : class
            {
                for (int i = 0; i < List.Count; i++)
                    if (List[i] is T)
                        return List[i] as T;

                return null;
            }
            public virtual void Find<T>(out T target)
                where T : class
            {
                target = Find<T>();
            }

            public BaseCollection(GameObject root)
            {
                List = Dependancy.GetAll<TType>(root);
            }
        }

        public class Collection : BaseCollection<IReference>
        {
            public virtual void Configure<TReference>(TReference reference)
                where TReference : class
            {
                ForAll<IReference<TReference>>(Process);

                void Process(IReference<TReference> instance) => References.Configure(reference, instance);
            }

            public virtual void Init<TReference>(TReference reference)
                where TReference : class
            {
                ForAll<IReference<TReference>>(Process);

                void Process(IReference<TReference> instance) => References.Init(reference, instance);
            }

            public Collection(GameObject root) : base(root)
            {

            }
        }
    }

    public interface IReference
    {
        void Init();
    }
    public interface IReference<T> : IReference
    {
        void Configure(T reference);
    }

    public class ReferenceBehaviour<TReference> : MonoBehaviour, IReference<TReference>
    {
        public TReference Reference { get; protected set; }

        public virtual void Configure(TReference reference)
        {
            this.Reference = reference;
        }

        public virtual void Init()
        {

        }
    }
}