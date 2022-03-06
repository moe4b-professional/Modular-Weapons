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
	public class WeaponAnimationEffectsWeight : WeaponAnimationEffects.Module
    {
		[SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float target = 1f;
        public float Target
        {
            get => target;
            set => target = Mathf.Clamp01(value);
        }

        public float Value
        {
            get => Animator.GetLayerWeight(LayerIndex);
            set => Animator.SetLayerWeight(LayerIndex, value);
        }

        public const string LayerName = "Effects";
        public int LayerIndex { get; protected set; }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Activation.OnEnable += EnableCallback;

            Weapon.OnProcess += Process;
        }

        void EnableCallback()
        {
            LayerIndex = Animator.GetLayerIndex(LayerName);
        }

        void Process()
        {
            Value = Mathf.MoveTowards(Value, target * Effects.Scale.Value, speed * Time.deltaTime);
        }
    }
}