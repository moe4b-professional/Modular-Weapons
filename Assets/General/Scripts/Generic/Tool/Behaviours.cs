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
    public static class Behaviours
    {
        [Serializable]
        public class Collection<TReference> : ReferencedCollection<TReference, IBehaviour<TReference>>
            where TReference : Component
        {
            public Collection(TReference reference) : base(reference) { }
        }
    }

    public interface IBehaviour<TReference> : ReferenceCollection.IElement
    {
        
    }
}