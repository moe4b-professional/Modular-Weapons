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

        public const string AnimatorFieldName = "Aim";

        public virtual bool Value
        {
            get => animator.GetBool(AnimatorFieldName);
            set => Animator.SetBool(AnimatorFieldName, value);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            Debug.Log(Value);

            if(data is PlayerWeaponsProcess.IData)
            {
                var playerData = data as PlayerWeaponsProcess.IData;

                Process(playerData);
            }
        }

        public virtual void Process(PlayerWeaponsProcess.IData data)
        {
            Value = data.SecondaryInput;
        }
	}
}