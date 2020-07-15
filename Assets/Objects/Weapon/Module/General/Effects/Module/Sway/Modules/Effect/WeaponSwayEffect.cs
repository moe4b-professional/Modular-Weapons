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

        public abstract Vector3 Effect { get; }
        
        public Vector3 Offset { get; protected set; }

        public Transform Context => Sway.Context;

        public WeaponSway.IProcessor Processor => Sway.Processor;

        public Transform Anchor => Processor.Anchor;

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
            CalculateOffset();

            Apply();
        }

        protected abstract void CalculateOffset();

        protected abstract void Apply();
    }
}