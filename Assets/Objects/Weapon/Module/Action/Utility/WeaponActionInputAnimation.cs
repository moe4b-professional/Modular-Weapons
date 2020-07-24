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
	public class WeaponActionInputAnimation : Weapon.Module
	{
        public const string ID = "Input";

        public Animator Animator => Weapon.Mesh.Animator;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            Animator.SetBool(ID, Weapon.Action.Input.Button.Held);
        }
    }
}