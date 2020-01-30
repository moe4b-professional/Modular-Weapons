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
        protected Animator animator;
        public Animator Animator { get { return animator; } }

        public int AnimatorLayerIndex => Animator.GetLayerIndex("Aim");

        [SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

        public bool IsOn { get; protected set; }

        public float Value
        {
            get => animator.GetLayerWeight(AnimatorLayerIndex);
            set => animator.SetLayerWeight(AnimatorLayerIndex, value);
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
            if (data.SecondaryInput.Press)
                IsOn = !IsOn;

            Value = Mathf.MoveTowards(Value, Target, speed * Time.deltaTime);
        }
	}
}