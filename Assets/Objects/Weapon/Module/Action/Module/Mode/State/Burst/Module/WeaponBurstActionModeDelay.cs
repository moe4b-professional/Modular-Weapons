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
	public class WeaponBurstActionModeDelay : WeaponBurstActionMode.Module, WeaponConstraint.IInterface
	{
		[SerializeField]
        protected float value = 0.3f;
        public float Value { get { return value; } }

        public float Timer { get; protected set; }

        public bool Active => Timer > 0f;

        bool WeaponConstraint.IInterface.Active => Active;

        public override void Init()
        {
            base.Init();

            Burst.OnEnd += EndCallback;

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            if(Timer != 0f)
            {
                Timer = Mathf.MoveTowards(Timer, 0f, Time.deltaTime);

                if (enabled == false) Timer = 0f;
            }
        }

        void EndCallback()
        {
            if (enabled) Timer = value;
        }
    }
}