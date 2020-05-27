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
	public class WeaponAnimationEffects : Weapon.Module<WeaponAnimationEffects.IProcessor>
	{
        public WeaponMesh Mesh => Weapon.Mesh;

        public Animator Animator => Mesh.Animator;

        public override void Init()
        {
            base.Init();

            if(HasProcessor)
            {
                Processor.OnJump += JumpCallback;

                Processor.OnLand += LandCallback;
            }
        }

        void JumpCallback()
        {
            Animator.SetTrigger("Jump");
        }

        void LandCallback()
        {
            Animator.SetTrigger("Land");
        }

        public interface IProcessor
        {
            event JumpDelegate OnJump;

            event LandDelegate OnLand;
        }

        public delegate void JumpDelegate();

        public delegate void LandDelegate();
    }
}