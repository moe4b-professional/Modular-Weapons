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
	public class EntityHealth : Entity.Module
	{
		[SerializeField]
        protected float value;
        public float Value
        {
            get => value;
            set
            {
                if (value < 0f) value = 0f;

                var previous = this.value;

                this.value = value;

                OnValueChange?.Invoke(this.value, previous);
            }
        }

        public delegate void ChangeDelegate(float current, float previous);
        public event ChangeDelegate OnValueChange;

        [SerializeField]
        protected float max = 100f;
        public float Max { get { return max; } }
        
        protected virtual void Reset()
        {
            value = max;
        }
    }
}