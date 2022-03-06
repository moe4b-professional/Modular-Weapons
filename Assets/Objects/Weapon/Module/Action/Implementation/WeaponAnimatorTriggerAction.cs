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
        public Animator Animator => Weapon.Mesh.Animator;

        [SerializeField]
        protected string trigger;
        public string Trigger { get { return trigger; } }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Action.OnPerform += Action;
        }

        void Action()
        {
            if(enabled) Animator.SetTrigger(trigger);
        }
    }
}