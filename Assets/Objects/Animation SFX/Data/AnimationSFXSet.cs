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
    [CreateAssetMenu]
	public class AnimationSFXSet : ScriptableObject
	{
        [SerializeField]
        protected Element[] list;
        public Element[] List { get { return list; } }
        [Serializable]
        public class Element
        {
            [SerializeField]
            protected string name;
            public string Name { get { return name; } }

            [SerializeField]
            protected AudioClip clip;
            public AudioClip Clip { get { return clip; } }
        }
        
        public virtual Element Find(string name)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i].Name == name)
                    return list[i];

            return null;
        }
	}
}