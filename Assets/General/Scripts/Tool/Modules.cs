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
    public static class Modules
    {
        public static void Set<TReference>(TReference reference, IReference<TReference> target)
        {
            target.Setup(reference);
        }
        public static void Configure<TReference>(TReference reference, IInitialization<TReference> target)
        {
            target.Configure();
        }
        public static void Init<TReference>(TReference reference, IInitialization<TReference> target)
        {
            target.Init();
        }

        public static void Process<TReference>(TReference reference, IModule<TReference> target)
        {
            Set(reference, target);
            Configure(reference, target);
            Init(reference, target);
        }

        public class Collection<TReference, TModule>
            where TReference : Component
            where TModule : class, IReference<TReference>
        {
            public List<TModule> List { get; protected set; }

            public TReference Reference { get; protected set; }

            public virtual void Set()
            {
                ForAll(Process);

                void Process(TModule instance) => Modules.Set(Reference, instance);
            }
            public virtual void Configure()
            {
                Set();

                ForAll<IInitialization<TReference>>(Process);

                void Process(IInitialization<TReference> instance) => Modules.Configure(Reference, instance);
            }
            public virtual void Init()
            {
                ForAll<IInitialization<TReference>>(Process);

                void Process(IInitialization<TReference> instance) => Modules.Init(Reference, instance);
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

            public virtual void Register(TModule instance)
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
        void Setup(T reference);
    }

    public interface IInitialization<T>
    {
        void Configure();

        void Init();
    }

    public interface IModule<T> : IReference<T>, IInitialization<T>
    {

    }

    public class MonoBehaviourModule<TReference> : MonoBehaviour, IModule<TReference>
    {
        public TReference Reference { get; protected set; }
        public virtual void Setup(TReference reference)
        {
            this.Reference = reference;
        }

        public virtual void Configure()
        {

        }

        public virtual void Init()
        {

        }
    }
}