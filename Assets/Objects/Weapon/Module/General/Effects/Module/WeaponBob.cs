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
	public class WeaponBob : Weapon.Module, WeaponEffects.IInterface
    {
        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected float range = 0.0035f;
        public float Range { get { return range; } }

        public Vector3 Offset { get; protected set; }

        public Transform Context => Pivot.transform;

        public WeaponPivot Pivot => Weapon.Pivot;

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            Vector3 Delta { get; }
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Pivot.OnProcess += Process;
        }

        void Process()
        {
            CalculateOffset();

            Context.localPosition += Offset;
        }

        protected virtual void CalculateOffset()
        {
            Offset = Vector3.zero;

            if (enabled)
                Offset -= Processor.Delta * range * scale;
        }
	}
}