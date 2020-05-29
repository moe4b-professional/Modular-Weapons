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

        public LayerController HipLayer { get; protected set; }
        public LayerController AimLayer { get; protected set; }
        public class LayerController
        {
            public string Name { get; protected set; }

            public int Index { get; protected set; }

            public Animator Animator { get; protected set; }

            public float Weight
            {
                get => Animator.GetLayerWeight(Index);
                set => Animator.SetLayerWeight(Index, value);
            }

            public virtual void CalculateIndex()
            {
                Index = Animator.GetLayerIndex(Name);
            }

            public LayerController(Animator animator, string name)
            {
                this.Animator = animator;
                this.Name = name;
            }
        }
        
        public override void Init()
        {
            base.Init();

            HipLayer = new LayerController(Animator, "Hip");

            AimLayer = new LayerController(Animator, "Aim");

            Aim.OnRateChange += RateChangeCallback;

            Weapon.Activation.OnEnable += EnableCallback;
        }

        void EnableCallback()
        {
            HipLayer.CalculateIndex();

            AimLayer.CalculateIndex();

            UpdateState();
        }

        void RateChangeCallback(float rate) => UpdateState();

        protected virtual void UpdateState()
        {
            HipLayer.Weight = Aim.InverseRate;
            AimLayer.Weight = Aim.Rate;
        }
    }

    public enum InputAggregationMode
    {
        Hold, Toggle
    }
}