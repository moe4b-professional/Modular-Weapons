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

using MB;

namespace Game
{
    public class WeaponBurstActionMode : Weapon.Module, WeaponActionOverride.IInterface, WeaponActionMode.IState
    {
        [SerializeField]
        protected int iterations = 3;
        public int Iterations { get { return iterations; } }

        public int Counter { get; protected set; }

        public float Input => Counter > 0 ? 1f : 0f;

        public bool IsProcessing => Weapon.Action.Override.Is(this);

        [field: SerializeField, DebugOnly]
        public Modules<WeaponBurstActionMode> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponBurstActionMode>
        {
            [field: SerializeField, DebugOnly]
            public WeaponBurstActionMode Burst { get; protected set; }

            public Weapon Weapon => Burst.Weapon;

            public virtual void Set(WeaponBurstActionMode value) => Burst = value;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponBurstActionMode>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Action.OnPerform += Action;

            Weapon.Activation.OnDisable += DisableCallback;
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