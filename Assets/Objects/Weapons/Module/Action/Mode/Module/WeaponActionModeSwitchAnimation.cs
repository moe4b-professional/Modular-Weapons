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
            
            Mode.OnChange += ChangeCallback;

            Mesh.TriggerRewind.Add(End, trigger + " End");

            Weapon.Activation.OnDisable += DisableCallback;
        }

        void DisableCallback()
        {
            if(Active) End();
        }

        void ChangeCallback(int index, WeaponActionMode.IState module)
        {
            Begin();
        }

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