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
    public class ForceData
    {
        [SerializeField]
        protected float value;
        public float Value { get { return value; } }

        [SerializeField]
        protected ForceMode mode;
        public ForceMode Mode { get { return mode; } }

        public ForceData(float value, ForceMode mode)
        {
            this.value = value;

            this.mode = mode;
        }
    }
}