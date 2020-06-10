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
	public class WeaponAnimationLandEffect : WeaponAnimationEffects.Module
    {
        [SerializeField]
        protected WeightData weight = WeightData.Create(9f);
        public WeightData Weight { get { return weight; } }
        [Serializable]
        public struct WeightData
        {
            [SerializeField]
            float scale;
            public float Scale { get { return scale; } }

            [SerializeField]
            AnimationCurve curve;
            public AnimationCurve Curve { get { return curve; } }

            public float Evaluate(Vector3 velocity) => curve.Evaluate(-velocity.y / scale);

            public WeightData(float scale, AnimationCurve curve)
            {
                this.scale = scale;
                this.curve = curve;
            }

            public static WeightData Create(float scale)
            {
                var curve = new AnimationCurve()
                {
                    keys = new Keyframe[]
                    {
                        new Keyframe(0f, 0f),
                        new Keyframe(1f, 1f),
                    },
                };

                return new WeightData(scale, curve);
            }
        }

        public const string Trigger = "Land";

        public override void Init()
        {
            base.Init();

            Weapon.Activation.OnEnable += EnableCallback;
            Weapon.Activation.OnDisable += DisableCallback;
        }

        void EnableCallback()
        {
            Effects.Processor.OnLand += LandCallback;
        }
        void DisableCallback()
        {
            Effects.Processor.OnLand -= LandCallback;
        }

        void LandCallback(Vector3 relativeVelocity)
        {
            Effects.Play(Trigger, weight.Evaluate(relativeVelocity));
        }
    }
}