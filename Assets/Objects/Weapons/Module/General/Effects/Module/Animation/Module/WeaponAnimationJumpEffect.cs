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
	public class WeaponAnimationJumpEffect : WeaponAnimationEffects.Module
    {
        [SerializeField]
        protected WeightData weight = new WeightData(1f);
        public WeightData Weight { get { return weight; } }
        [Serializable]
        public struct WeightData
        {
            [SerializeField]
            [Range(0f, 1f)]
            float initial;
            public float Initial { get { return initial; } }

            [SerializeField]
            [Range(0f, 1f)]
            float recurring;
            public float Recurring { get { return recurring; } }

            public float Evaluate(int count) => count > 1 ? recurring : initial;

            public WeightData(float initial, float recurring)
            {
                this.initial = initial;
                this.recurring = recurring;
            }
            public WeightData(float value) : this(value, value / 2f) { }
        }

        public const string Trigger = "Jump";

        public override void Init()
        {
            base.Init();

            if (Effects.HasProcessor)
            {
                Weapon.Activation.OnEnable += EnableCallback;
                Weapon.Activation.OnDisable += DisableCallback;
            }
        }

        void EnableCallback()
        {
            Processor.OnJump += JumpCallback;
        }
        void DisableCallback()
        {
            Processor.OnJump -= JumpCallback;
        }

        void JumpCallback(int count)
        {
            Effects.Play(Trigger, weight.Evaluate(count));
        }
    }
}