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

        public string FillTrigger => AnimationTrigger.Combine(ID, nameof(Fill));

        public override void Init()
        {
            base.Init();

            Mesh.TriggerRewind.OnTrigger += AnimationEventCallback;

            Reload.OnPerform += PerformCallback;
        }

        void PerformCallback()
        {
            Animator.SetTrigger(ID);
        }

        void AnimationEventCallback(string trigger)
        {
            if (trigger == FillTrigger) Fill();

            if (AnimationTrigger.End.Is(trigger, ID)) End();
        }

        protected virtual void Fill()
        {
            Animator.SetTrigger(FillTrigger);

            Reload.Refill();
        }

        protected virtual void End()
        {
            Animator.ResetTrigger(FillTrigger);

            Reload.Complete();
        }
    }
}