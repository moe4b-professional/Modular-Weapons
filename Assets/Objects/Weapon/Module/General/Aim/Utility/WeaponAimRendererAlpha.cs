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
	public class WeaponAimRendererAlpha : WeaponAim.Module
	{
		[SerializeField]
        protected Renderer target;
        public Renderer Target { get { return target; } }

        [SerializeField]
        protected ValueRange scale = new ValueRange(0.2f, 1f);
        public ValueRange Scale { get { return scale; } }

        [SerializeField]
        protected AnimationCurve curve;
        public AnimationCurve Curve { get { return curve; } }

        public override void Init()
        {
            base.Init();

            Aim.OnRateChange += RateChaneCallback;

            UpdateState();
        }

        void RateChaneCallback(float rate) => UpdateState();

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

            var eval = curve.Evaluate(Aim.InverseRate);

            color.a = scale.Lerp(eval);

            return color;
        }
    }
}