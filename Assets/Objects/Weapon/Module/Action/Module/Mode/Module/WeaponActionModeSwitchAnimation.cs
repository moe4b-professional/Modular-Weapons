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
    public class WeaponActionModeSwitchAnimation : WeaponActionMode.Module, WeaponConstraint.IInterface
    {
        public const string ID = "Switch Action Mode";

        public bool Active { get; protected set; }
        
        public WeaponMesh Mesh => Weapon.Mesh;
        
        public override void Initialize()
        {
            base.Initialize();

            Weapon.Activation.OnDisable += DisableCallback;

            Mesh.TriggerRewind.OnTrigger += AnimationTriggerCallback;

            Mode.OnChange += ChangeCallback;
        }

        void DisableCallback()
        {
            if (Active) End();
        }

        void AnimationTriggerCallback(string trigger)
        {
            if (AnimationTrigger.End.Is(trigger, ID)) End();
        }

        void ChangeCallback(int index, WeaponActionMode.IState module) => Begin();

        protected virtual void Begin()
        {
            Active = true;

            Mesh.Animator.SetTrigger(ID);
        }

        protected virtual void End()
        {
            Active = false;
        }
    }
}