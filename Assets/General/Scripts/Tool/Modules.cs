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
        public static void Setup<TReference>(TReference reference, IReference<TReference> target)
            where TReference : Component
        {
            target.Setup(reference);
        }

        public static void Configure<TReference>(TReference reference, IModule<TReference> target)
            where TReference : Component
        {
            target.Configure();
        }

        static List<object> init;

        public static void Init<TReference>(TReference reference, IModule<TReference> target)
            where TReference : Component
        {
            target.Init();

            if (init == null) init = new List<object>();

            var text = AnimationUtility.CalculateTransformPath(target.transform, reference.transform);

            if (init.Contains(target))
            {
                Debug.LogError("Duplicate Initialization of: " + text, target.transform.gameObject);
                Debug.LogError("Initialization By: " + reference.name, reference.gameObject);
            }
            else
                init.Add(target);
        }

        public static void Process<TReference>(TReference reference, IModule<TReference> target)
            where TReference : Component
        {
            Setup(reference, target);
            Configure(reference, target);
            Init(reference, target);
        }

        [Serializable]
        public class Collection<TReference> : ReferencedCollection<TReference, IReference<TReference>>
            where TReference : Component
        {
            public virtual void Setup()
            {
                ForAll(Process);

                void Process(IReference<TReference> instance) => Modules.Setup(Reference, instance);
            }
            public virtual void Configure()
            {
                Setup();

                ForAll<IModule<TReference>>(Process);

                void Process(IModule<TReference> instance) => Modules.Configure(Reference, instance);
            }
            public virtual void Init()
            {
                ForAll<IModule<TReference>>(Process);

                void Process(IModule<TReference> instance) => Modules.Init(Reference, instance);
            }
            
            public Collection(TReference reference) : base(reference) { }
        }
    }

    public interface IReference<T> : ReferenceCollection.IElement
    {
        void Setup(T reference);
    }
    
    public interface IModule<T> : IReference<T>
    {
        void Configure();

        void Init();
    }
}