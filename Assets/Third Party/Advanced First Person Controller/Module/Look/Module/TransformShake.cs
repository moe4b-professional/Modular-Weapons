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
	public class TransformShake : MonoBehaviour
	{
        [SerializeField]
        protected float range = 5f;
        public float Scale { get { return range; } }

        [SerializeField]
        protected float reset = 2f;
        public float Reset { get { return reset; } }

        public float Value { get; protected set; }

        public float Impact => range * Mathf.Pow(Value, 2);

        public Quaternion Offset { get; protected set; }

        [SerializeField]
        protected ContextData[] contexts;
        public ContextData[] Contexts { get { return contexts; } }
        [Serializable]
        public class ContextData
        {
            [SerializeField]
            protected AnchoredTransform target;
            public AnchoredTransform Target { get { return target; } }

            public Transform Context => target.transform;

            [SerializeField]
            protected bool invert;
            public bool Invert { get { return invert; } }

            public Quaternion Offset { get; protected set; }

            public TransformShake Shake { get; protected set; }
            public void Configure(TransformShake reference)
            {
                Shake = reference;

                target.AfterWriteDefaults += AfterWriteDefaultsCallback;
            }

            void AfterWriteDefaultsCallback()
            {
                if (invert)
                    Offset = Quaternion.Inverse(Shake.Offset);
                else
                    Offset = Shake.Offset;

                Context.localRotation *= Offset;
            }
        }

        protected virtual void Awake()
        {
            for (int i = 0; i < contexts.Length; i++)
                contexts[i].Configure(this);
        }

        public virtual void Calculate()
        {
            Value = Mathf.MoveTowards(Value, 0f, reset * Time.deltaTime);

            Offset = Quaternion.Euler(Impact * GetNoise(1), Impact * GetNoise(2), 0f);
        }

        public virtual void Add(float target) => Value += target;

        public virtual void Set(float target) => Value = target;

        protected virtual float GetNoise(int seed)
        {
            var perlin = Mathf.PerlinNoise(Time.time + seed, Time.time + seed);

            return Mathf.Lerp(-1f, 1f, perlin);
        }
    }
}