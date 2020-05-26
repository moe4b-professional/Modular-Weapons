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
	public class WeaponAnimatorLayerAim : WeaponAim.Module
    {
        public Animator Animator => Weapon.Mesh.Animator;

        [SerializeField]
        protected string layerName = "Aim";
        public string LayerName { get { return layerName; } }

        public int LayerIndex { get; protected set; }

        public float LayerWeight
        {
            get => Animator.GetLayerWeight(LayerIndex);
            set => Animator.SetLayerWeight(LayerIndex, value);
        }

        public override void Init()
        {
            base.Init();

            Aim.OnRateChange += RateChangeCallback;

            Weapon.Activation.OnEnable += EnableCallback;
        }

        void EnableCallback()
        {
            LayerIndex = Animator.GetLayerIndex(layerName);

            UpdateState();
        }

        void RateChangeCallback(float rate) => UpdateState();

        protected virtual void UpdateState()
        {
            LayerWeight = Aim.Rate;
        }
    }

    public enum InputAggregationMode
    {
        Hold, Toggle
    }
}