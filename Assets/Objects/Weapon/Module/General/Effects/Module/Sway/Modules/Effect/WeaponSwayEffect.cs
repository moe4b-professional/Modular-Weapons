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
    public abstract class WeaponSwayEffect : WeaponSway.Module
    {
        [SerializeField]
        protected float multiplier = 1f;
        public float Multiplier { get { return multiplier; } }

        [SerializeField]
        protected EffectData effect;
        public EffectData Effect { get { return effect; } }
        [Serializable]
        public struct EffectData
        {
            [SerializeField]
            float vertical;
            public float Vertical { get { return vertical; } }

            [SerializeField]
            float horizontal;
            public float Horizontal { get { return horizontal; } }

            [SerializeField]
            float fordical;
            public float Fordical { get { return fordical; } }

            public EffectData(float vertical, float horizontal, float fordical)
            {
                this.vertical = vertical;
                this.horizontal = horizontal;
                this.fordical = fordical;
            }
        }
        
        public Vector3 Offset { get; protected set; }

        public Transform Context => Sway.Context;

        public WeaponSway.IProcessor Processor => Sway.Processor;

        protected virtual void Reset()
        {

        }

        public override void Configure()
        {
            base.Configure();

            Offset = Vector3.zero;
        }

        public override void Init()
        {
            base.Init();

            Sway.OnProcess += Process;
        }

        protected virtual void Process()
        {
            SampleOffset();

            Offset *= Sway.Scale.Value * multiplier;

            if (enabled == false) Offset = Vector3.zero;

            AdjustSpace();

            Apply();
        }

        protected abstract void SampleOffset();

        protected virtual void AdjustSpace()
        {
            var localUp = Weapon.transform.InverseTransformDirection(Processor.Anchor.up);

            Offset = Vector3.ProjectOnPlane(Offset, localUp);
        }

        protected abstract void Apply();
    }
}