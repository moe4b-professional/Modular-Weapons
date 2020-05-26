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
	public class WeaponSprintAnimation : WeaponSprint.Module
	{
        [SerializeField]
        protected string trigger = "Sprint";
        public string Trigger { get { return trigger; } }

        public Animator Animator => Weapon.Mesh.Animator;
        
        public override void Init()
        {
            base.Init();
            
            Sprint.OnBegin += BeginCallback;
            Sprint.OnStop += StopCallback;
        }

        void BeginCallback()
        {
            Animator.SetBool(trigger, true);
        }

        void StopCallback()
        {
            Animator.SetBool(trigger, false);
        }
    }
}