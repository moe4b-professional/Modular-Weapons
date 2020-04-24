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
	public class WeaponPlayerAim : Weapon.Module
	{
        [SerializeField]
        protected InputAggregationMode inputMode = InputAggregationMode.Toggle;
        public InputAggregationMode InputMode { get { return inputMode; } }

        public Animator Animator => Weapon.Animator;

        public int AnimatorLayerIndex => Animator.GetLayerIndex("Aim");

        [SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

        public bool IsOn { get; protected set; }

        public float Value
        {
            get => Animator.GetLayerWeight(AnimatorLayerIndex);
            set => Animator.SetLayerWeight(AnimatorLayerIndex, value);
        }

        public float Target => IsOn ? 1f : 0f;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if(data is PlayerWeaponsProcess.IData)
            {
                var playerData = data as PlayerWeaponsProcess.IData;

                Process(playerData);
            }
        }

        public virtual void Process(PlayerWeaponsProcess.IData data)
        {
            if(inputMode == InputAggregationMode.Toggle)
            {
                if (data.SecondaryButton.Press) IsOn = !IsOn;
            }
            else if(inputMode == InputAggregationMode.Hold)
            {
                IsOn = data.SecondaryButton.Held;
            }

            Value = Mathf.MoveTowards(Value, Target, speed * Time.deltaTime);
        }
	}

    public enum InputAggregationMode
    {
        Hold, Toggle
    }
}