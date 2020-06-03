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
        public static void Configure<TReference>(TReference reference, IList<IReference<TReference>> targets)
        {
            for (int i = 0; i < targets.Count; i++)
                Configure(reference, targets[i]);
        }
        public static void Configure<TReference>(TReference reference, IReference<TReference> target)
        {
            target.Configure(reference);
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

        public class Collection<TReference, TModule>
            where TReference : Component
            where TModule : class, IReference<TReference>
        {
            public List<TModule> List { get; protected set; }

            public TReference Reference { get; protected set; }

            public virtual void Configure()
            {
                ForAll(Process);

                void Process(TModule instance) => References.Configure(Reference, instance);
            }

            public virtual void Init()
            {
                ForAll(Process);

                void Process(TModule instance) => References.Init(Reference, instance);
            }

            public virtual void ForAll(Action<TModule> action)
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

            public virtual void Add(TModule instance)
            {
                List.Add(instance);
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
            
            public Collection(TReference reference, GameObject root)
            {
                this.Reference = reference;

                List = Dependancy.GetAll<TModule>(root);
            }
            public Collection(TReference reference) : this(reference, reference.gameObject) { }
        }

        public class Collection<TReference> : Collection<TReference, IReference<TReference>>
            where TReference : Component
        {
            public Collection(TReference reference, GameObject root) : base(reference, root) { }
            public Collection(TReference reference) : base(reference, reference.gameObject) { }
        }
    }

    public interface IReference<T>
    {
        void Init();

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