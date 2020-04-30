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
	public class WeaponAimScaleModifier : Weapon.Module
	{
        [SerializeField]
        protected float min = 0.2f;
        public float Min { get { return min; } }

        [SerializeField]
        protected float max = 1f;
        public float Max { get { return max; } }

        public WeaponAim Aim { get; protected set; }

        public WeaponSway Sway { get; protected set; }
        public WeaponBob Bob { get; protected set; }
        public WeaponRecoil Recoil { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Aim = Weapon.GetComponentInChildren<WeaponAim>();

            Sway = Weapon.GetComponentInChildren<WeaponSway>();

            Bob = Weapon.GetComponentInChildren<WeaponBob>();

            Recoil = Weapon.GetComponentInChildren<WeaponRecoil>();
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

            if (Sway != null) Sway.Scale = value;

            if (Bob != null) Bob.Scale = value;

            if (Recoil != null) Recoil.Scale = value;
        }
    }
}