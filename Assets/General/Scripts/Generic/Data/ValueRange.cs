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
    public struct ValueRange
    {
        [SerializeField]
        float min;
        public float Min { get { return min; } }

        [SerializeField]
        float max;
        public float Max { get { return max; } }

        public float Random => UnityEngine.Random.Range(min, max);

        public float Lerp(float t) => Mathf.Lerp(min, max, t);

        public float Clamp(float value) => Mathf.Clamp(value, min, max);

        public ValueRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}