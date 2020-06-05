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
    public class WeaponBurstActionMode : Weapon.Module, WeaponActionOverride.IInterface, WeaponActionMode.IState
    {
        [SerializeField]
        protected int iterations = 3;
        public int Iterations { get { return iterations; } }

        public int Counter { get; protected set; }

        bool WeaponActionOverride.IInterface.Input => Counter > 0;

        public bool IsProcessing => Weapon.Action.Override.Is(this);

        public abstract class Module : Weapon.BaseModule<WeaponBurstActionMode>
        {
            public WeaponBurstActionMode Burst => Reference;

            public override Weapon Weapon => Burst.Weapon;
        }

        public Modules.Collection<WeaponBurstActionMode> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<WeaponBurstActionMode>(this, Weapon.gameObject);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Action.OnPerform += Action;

            Weapon.Activation.OnDisable += DisableCallback;

            Modules.Init();
        }

        void DisableCallback()
        {
            if (IsProcessing) Stop();
        }
        protected virtual void OnDisable()
        {
            if (IsProcessing) Stop();
        }
        
        void Action()
        {
            if (enabled)
            {
                if (Weapon.Action.Override.Is(null))
                    Begin();
            }

            if (IsProcessing)
            {
                Counter--;

                if (Counter == 0) End();
            }
        }

        public event Action OnBegin;
        protected virtual void Begin()
        {
            Weapon.Action.Override.Set(this);

            Counter = iterations;

            OnBegin?.Invoke();
        }

        public event Action OnEnd;
        protected virtual void End()
        {
            Stop();

            OnEnd?.Invoke();
        }

        public virtual void Stop()
        {
            Counter = 0;

            Weapon.Action.Override.Clear(this);
        }
    }
}