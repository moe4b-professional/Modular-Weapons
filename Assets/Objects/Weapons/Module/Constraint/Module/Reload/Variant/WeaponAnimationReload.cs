﻿using System;
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
	public class WeaponAnimationReload : WeaponReload.Module
	{
        [SerializeField]
        protected string _ID = "Reload";
        public string ID { get { return _ID; } }

        public WeaponMesh Mesh => Weapon.Mesh;

        public override void Init()
        {
            base.Init();

            Mesh.TriggerRewind.OnTrigger += AnimationEventCallback;

            Reload.OnPerform += PerformCallback;
        }

        void PerformCallback()
        {
            Mesh.Animator.SetTrigger(ID);
        }

        void AnimationEventCallback(string trigger)
        {
            if (AnimationTrigger.Is(trigger, ID, "Fill")) Fill();

            if (AnimationTrigger.End.Is(trigger, ID)) End();
        }

        protected virtual void Fill() => Reload.Refill();

        protected virtual void End() => Reload.Complete();
    }
}