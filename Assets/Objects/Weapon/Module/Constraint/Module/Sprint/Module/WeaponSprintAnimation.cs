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
        public Animator Animator => Weapon.Mesh.Animator;

        public const string ID = "Sprint";

        public const string Toggle = ID + " Toggle";

        public const string Trigger = ID + " Trigger";

        public override void Init()
        {
            base.Init();
            
            Sprint.OnBegin += BeginCallback;
            Sprint.OnStop += StopCallback;
        }

        void BeginCallback()
        {
            Animator.SetBool(Toggle, true);
            Animator.SetTrigger(Trigger);
        }

        void StopCallback()
        {
            Animator.SetBool(Toggle, false);
        }
    }
}