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
        public static IList<IModule<TReference>> Configure<TReference>(TReference reference)
            where TReference : Component
        {
            var targets = Retrieve(reference);

            Configure(reference, targets);

            return targets;
        }
        public static void Configure<TReference>(TReference reference, IList<IModule<TReference>> list)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].Configure(reference);
        }

        public static IList<IModule<TReference>> Init<TReference>(TReference reference)
            where TReference : Component
        {
            var targets = Retrieve(reference);

            Init(reference, targets);

            return targets;
        }
        public static void Init<TReference>(TReference reference, IList<IModule<TReference>> list)
        {
            for (int i = 0; i < list.Count; i++)
                list[i].Init();
        }

        public static IList<IModule<TReference>> Retrieve<TReference>(TReference reference)
            where TReference : Component
        {
            return Retrieve<TReference>(reference.gameObject);
        }
        public static IList<IModule<TReference>> Retrieve<TReference>(GameObject gameObject)
        {
            return gameObject.GetComponentsInChildren<IModule<TReference>>();
        }
    }

    public interface IModule<TReference>
    {
        void Configure(TReference reference);

        void Init();
    }

    public abstract class Module<TReference> : MonoBehaviour, IModule<TReference>
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