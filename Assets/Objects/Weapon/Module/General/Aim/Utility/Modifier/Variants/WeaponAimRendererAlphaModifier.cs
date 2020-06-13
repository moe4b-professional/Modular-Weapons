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
        protected AnimationCurve curve;
        public AnimationCurve Curve { get { return curve; } }

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

            var eval = curve.Evaluate(Rate);

            color.a = Mathf.Lerp(range.Max, range.Min, eval);

            return color;
        }
    }
}