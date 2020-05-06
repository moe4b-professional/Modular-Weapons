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
    public class WeaponBurstActionMode : Weapon.Module, WeaponConstraint.IInterface, WeaponAction.IOverride, WeaponActionMode.IModule
    {
        [SerializeField]
        protected int iterations = 3;
        public int Iterations { get { return iterations; } }

        [SerializeField]
        protected float delay = 0.3f;
        public float Delay { get { return delay; } }

        [SerializeField]
        protected bool lockInput = true;
        public bool LockInput { get { return lockInput; } }

        public bool InputLock { get; protected set; }

        bool WeaponConstraint.IInterface.Constraint => InputLock;

        bool WeaponAction.IOverride.Input => Counter > 0;

        public bool IsProcessing => Equals(Weapon.Action.Override);
        
        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Action.OnLatePerform += LateAction;
        }

        void Process(Weapon.IProcessData data)
        {
            if (enabled)
            {
                if (InputLock)
                {
                    if (data.Input == false || lockInput == false)
                        InputLock = false;
                }
            }
            else
            {
                InputLock = false;
            }
        }

        public int Counter { get; protected set; }

        void LateAction()
        {
            if (enabled)
            {
                if (Weapon.Action.Override == null) Begin();
            }

            if (IsProcessing)
            {
                Counter--;

                if (Counter == 0 || Weapon.Constraint.CheckAny(IsBreaking))
                    End();
            }
        }

        public virtual bool IsBreaking(WeaponConstraint.IInterface target)
        {
            if (target.Constraint == false) return false;

            if (target is WeaponAmmo) return true;

            return false;
        }

        protected virtual void Begin()
        {
            Weapon.Action.Override = this;

            Counter = iterations;
        }

        protected virtual void End()
        {
            Counter = 0;

            if (lockInput) InputLock = true;

            StartCoroutine(Procedure());

            IEnumerator Procedure()
            {
                yield return new WaitForSeconds(delay);

                Weapon.Action.Override = null;
            }
        }
    }
}