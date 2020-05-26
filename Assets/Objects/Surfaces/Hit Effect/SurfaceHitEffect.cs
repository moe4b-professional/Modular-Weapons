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
    public class SurfaceHitEffect
    {
        [SerializeField]
        protected GameObject prefab;
        public GameObject Prefab { get { return prefab; } }

        [SerializeField]
        protected bool flip;
        public bool Flip { get { return flip; } }

        [SerializeField]
        protected OffsetData offset;
        public OffsetData Offset { get { return offset; } }
        [Serializable]
        public class OffsetData
        {
            [SerializeField]
            protected float depth;
            public float Depth { get { return depth; } }

            [SerializeField]
            protected Vector3 angle;
            public Vector3 Angle { get { return angle; } }

            public Quaternion Rotation => Quaternion.Euler(angle);
        }

        public virtual GameObject Spawn(Vector3 point, Vector3 normal, Transform parent)
        {
            var instance = Object.Instantiate(prefab);

            instance.transform.position = point + (normal * offset.Depth);
            instance.transform.rotation = Quaternion.LookRotation(normal * (Flip ? 1f : -1f)) * offset.Rotation;

            instance.transform.SetParent(parent);

            return instance;
        }
    }
}