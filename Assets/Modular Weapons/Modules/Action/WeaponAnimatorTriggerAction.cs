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
	public class WeaponAnimatorTriggerAction : Weapon.Module
	{
        [SerializeField]
        protected Animator animator;
        public Animator Animator { get { return animator; } }

        [SerializeField]
        protected string trigger;
        public string Trigger { get { return trigger; } }

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += Action;
        }

        void Action()
        {
            animator.SetTrigger(trigger);
        }
    }
}