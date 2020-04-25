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
	public class WeaponAimSwayScaleModifier : Weapon.Module
	{
        [SerializeField]
        protected float min = 0.2f;
        public float Min { get { return min; } }

        [SerializeField]
        protected float max = 1f;
        public float Max { get { return max; } }

        public WeaponAim Aim { get; protected set; }

        public WeaponSway Sway { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Aim = Weapon.GetComponentInChildren<WeaponAim>();

            Sway = Weapon.GetComponentInChildren<WeaponSway>();
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

            if (Sway == null)
            {
                Debug.LogError(FormatDependancyError<WeaponSway>());
                enabled = false;
                return;
            }

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            Sway.Scale = Mathf.Lerp(max, min, Aim.Rate);
        }
    }
}