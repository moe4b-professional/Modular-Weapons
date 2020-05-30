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
	public class WeaponBob : Weapon.Module<WeaponBob.IProcessor>, WeaponEffects.IInterface
    {
        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected AnimationCurve curve;
        public AnimationCurve Curve { get { return curve; } }

        [SerializeField]
        protected float range = 0.02f;
        public float Range { get { return range; } }

        public Vector3 Offset { get; protected set; }

        public Transform Context => Pivot.transform;

        public WeaponPivot Pivot => Weapon.Pivot;

        public override void Init()
        {
            base.Init();

            Pivot.OnProcess += Apply;
        }

        void Apply()
        {
            CalculateOffset();

            Context.localPosition += Offset;
        }

        protected virtual void CalculateOffset()
        {
            if (enabled && HasProcessor)
                Offset = Vector3.down * curve.Evaluate(Processor.Step) * range * scale;
            else
                Offset = Vector3.zero;
        }

        public interface IProcessor
        {
            float Step { get; }
        }
	}
}