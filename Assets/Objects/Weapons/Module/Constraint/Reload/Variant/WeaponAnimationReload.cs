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
	public class WeaponAnimationReload : WeaponReload
	{
        [SerializeField]
        protected string trigger = "Reload";
        public string Trigger { get { return trigger; } }

        public WeaponMesh Mesh => Weapon.Mesh;

        public override void Init()
        {
            base.Init();

            Mesh.TriggerRewind.OnTrigger += AnimationEventCallback;
        }

        void AnimationEventCallback(string ID)
        {
            if (ID == trigger + " Fill") Fill();

            if (ID == trigger + " End") End();
        }

        public override void Perform()
        {
            base.Perform();

            Mesh.Animator.SetTrigger(trigger);
        }

        protected virtual void Fill()
        {
            Ammo.Refill();
        }

        protected virtual void End()
        {
            if (IsProcessing) Complete();
        }
    }
}