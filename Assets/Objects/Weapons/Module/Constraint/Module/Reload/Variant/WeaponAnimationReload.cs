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
        public const string ID = "Reload";

        public WeaponMesh Mesh => Weapon.Mesh;

        public override void Init()
        {
            base.Init();

            Mesh.TriggerRewind.OnTrigger += AnimationEventCallback;
        }

        void AnimationEventCallback(string trigger)
        {
            if (AnimationTrigger.Is(trigger, ID, "Fill")) Fill();

            if (AnimationTrigger.End.Is(trigger, ID)) End();
        }

        public override void Perform()
        {
            base.Perform();

            Mesh.Animator.SetTrigger(ID);
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