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
        [SerializeField]
        protected string trigger = "Switch Action Mode";
        public string Trigger { get { return trigger; } }

        public bool Active { get; protected set; }
        bool WeaponConstraint.IInterface.Constraint => Active;
        
        public WeaponMesh Mesh => Weapon.Mesh;
        
        public override void Init()
        {
            base.Init();

            Weapon.Activation.OnDisable += DisableCallback;

            Mesh.TriggerRewind.OnTrigger += AnimationTriggerCallback;

            Mode.OnChange += ChangeCallback;
        }

        void DisableCallback()
        {
            if (Active) End();
        }

        void AnimationTriggerCallback(string ID)
        {
            if (ID == trigger + " End") End();
        }

        void ChangeCallback(int index, WeaponActionMode.IState module) => Begin();

        protected virtual void Begin()
        {
            Active = true;

            Mesh.Animator.SetTrigger(trigger);
        }

        protected virtual void End()
        {
            Active = false;
        }
    }
}