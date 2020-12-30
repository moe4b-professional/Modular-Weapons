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

        public float Blend { get; protected set; }

        public float Target
        {
            get
            {
                if (Weapon.Operation.Value != null) return 0f;

                return Weapon.Action.Input.Axis;
            }
        }

        public float Speed => 20f;

        public override void Configure()
        {
            base.Configure();

            Blend = 0f;
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            Blend = Mathf.MoveTowards(Blend, Target, Speed * Time.deltaTime);

            Animator.SetFloat(ID, Blend);
        }
    }
}