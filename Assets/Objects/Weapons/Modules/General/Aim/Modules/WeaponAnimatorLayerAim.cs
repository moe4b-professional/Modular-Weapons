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
	public class WeaponAnimatorLayerAim : Weapon.Module
    {
        public Animator Animator => Weapon.Animator;

        [SerializeField]
        protected string layerName = "Aim";
        public string LayerName { get { return layerName; } }

        public int LayerIndex { get; protected set; }

        public float LayerWeight
        {
            get => Animator.GetLayerWeight(LayerIndex);
            set => Animator.SetLayerWeight(LayerIndex, value);
        }

        public WeaponAim Aim { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Aim = Weapon.GetComponentInChildren<WeaponAim>();
        }

        public override void Init()
        {
            base.Init();

            if(Aim == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAim>());
                enabled = false;
                return;
            }

            LayerIndex = Animator.GetLayerIndex(layerName);

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            LayerWeight = Aim.Rate;
        }
    }

    public enum InputAggregationMode
    {
        Hold, Toggle
    }
}