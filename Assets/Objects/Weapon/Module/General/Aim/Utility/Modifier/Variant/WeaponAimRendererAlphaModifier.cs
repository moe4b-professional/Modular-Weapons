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
	public class WeaponAimRendererAlphaModifier : WeaponAimPropertyModifier
    {
        [SerializeField]
        protected Renderer target;
        public Renderer Target { get { return target; } }

        [SerializeField]
        protected ValueRange range;
        public ValueRange Range { get { return range; } }

        public virtual float Value
        {
            get
            {
                var lerp = Mathf.Lerp(range.Max, range.Min, Rate);

                var eval = curve.Evaluate(lerp);

                return eval;
            }
        }

        [SerializeField]
        protected AnimationCurveToggleValue curve;
        public AnimationCurveToggleValue Curve { get { return curve; } }

        protected override void Reset()
        {
            base.Reset();

            range = new ValueRange(0.2f, 1f);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            UpdateState();
        }

        void Process()
        {
            UpdateState();
        }

        protected virtual void UpdateState()
        {
            var block = new MaterialPropertyBlock();

            var color = SampleColor();

            block.SetColor("_Color", color);

            target.SetPropertyBlock(block);
        }

        protected virtual Color SampleColor()
        {
            var color = target.material.color;

            color.a = Value;

            return color;
        }
    }
}