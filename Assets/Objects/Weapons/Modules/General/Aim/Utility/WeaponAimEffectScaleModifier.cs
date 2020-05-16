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
	public class WeaponAimEffectScaleModifier : Weapon.Module
	{
        [SerializeField]
        protected float min = 0.2f;
        public float Min { get { return min; } }

        [SerializeField]
        protected float max = 1f;
        public float Max { get { return max; } }

        public WeaponAim Aim { get; protected set; }

        public IList<Weapon.IEffect> Targets { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Aim = Weapon.GetDependancy<WeaponAim>();

            Targets = Weapon.GetAllDependancies<Weapon.IEffect>();
        }

        public override void Init()
        {
            base.Init();

            if (Aim == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAim>());
                enabled = false;
                return;
            }

            Aim.OnRateChange += RateChangeCallback;

            UpdateState();
        }

        void RateChangeCallback(float rate)
        {
            UpdateState();
        }

        protected virtual void UpdateState()
        {
            var value = Mathf.Lerp(max, min, Aim.Rate);

            for (int i = 0; i < Targets.Count; i++)
                Targets[i].Scale = value;
        }
    }
}