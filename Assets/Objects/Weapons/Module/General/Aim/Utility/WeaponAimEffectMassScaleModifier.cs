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
	public class WeaponAimEffectMassScaleModifier : WeaponAim.Module
	{
        [SerializeField]
        protected float min = 0.2f;
        public float Min { get { return min; } }

        [SerializeField]
        protected float max = 1f;
        public float Max { get { return max; } }

        public IList<WeaponEffects.IInterface> Targets => Weapon.Effects.List;
        
        public override void Init()
        {
            base.Init();

            Aim.OnRateChange += RateChangeCallback;

            UpdateState();
        }

        void RateChangeCallback(float rate) => UpdateState();

        protected virtual void UpdateState()
        {
            var value = Mathf.Lerp(max, min, Aim.Rate);

            for (int i = 0; i < Targets.Count; i++)
                Targets[i].Scale = value;
        }
    }
}