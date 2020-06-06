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
	public class SurfaceHitEffectPack : ScriptableObject
	{
        [SerializeField]
        protected SurfaceHitEffect _default;
        public SurfaceHitEffect Default { get { return _default; } }

        [SerializeField]
        protected Element[] list;
        public Element[] List { get { return list; } }
        [Serializable]
        public class Element
        {
            [SerializeField]
            protected SurfaceMaterial[] materials;
            public SurfaceMaterial[] Materials { get { return materials; } }

            [SerializeField]
            protected SurfaceHitEffect data;
            public SurfaceHitEffect Data { get { return data; } }
        }

        public virtual SurfaceHitEffect Find(SurfaceMaterial material)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i].Materials.Contains(material))
                    return list[i].Data;

            return Default;
        }
    }
}