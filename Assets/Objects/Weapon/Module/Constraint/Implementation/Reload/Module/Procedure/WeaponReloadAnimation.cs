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
	public class WeaponReloadAnimation : WeaponReload.Procedure
    {
        [SerializeField]
        protected string _ID = "Reload";
        public string ID { get { return _ID; } }

        public WeaponMesh Mesh => Weapon.Mesh;
        public Animator Animator => Mesh.Animator;

        public override void Initialize()
        {
            base.Initialize();

            Mesh.TriggerRewind.Register($"{ID} Fill", Fill);
            Mesh.TriggerRewind.Register($"{ID} End", End);

            Reload.OnPerform += PerformCallback;
        }

        void PerformCallback()
        {
            Animator.SetTrigger(ID);
        }

        protected virtual void Fill()
        {
            Animator.SetTrigger($"{ID} Fill");

            Reload.Refill();
        }

        protected virtual void End()
        {
            Animator.ResetTrigger($"{ID} Fill");

            Reload.Complete();
        }
    }
}