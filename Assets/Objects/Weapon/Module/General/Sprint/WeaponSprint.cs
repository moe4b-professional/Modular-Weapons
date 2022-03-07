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
	public class WeaponSprint : Weapon.Module, WeaponOperation.IInterface
	{
        [SerializeField]
        protected float minWeight = 0.5f;
        public float MinWeight { get { return minWeight; } }

        public bool Active => Weapon.Operation.Is(this);

        [field: SerializeField, DebugOnly]
        public Modules<WeaponSprint> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponSprint>
        {
            [field: SerializeField, DebugOnly]
            public WeaponSprint Sprint { get; protected set; }

            public Weapon Weapon => Sprint.Weapon;

            public virtual void Set(WeaponSprint value) => Sprint = value;
        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            float Weight { get; }

            bool Active { get; }

            float Target { get; }
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponSprint>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;
        }

        void DisableCallback()
        {
            if (Active) Stop();
        }

        void Process()
        {
            if (Processor.Active && Processor.Target > minWeight)
            {
                if (Active == false)
                    Begin();
            }
            else
            {
                if (Active)
                    Stop();
            }
        }

        public event Action OnBegin;
        protected virtual void Begin()
        {
            Weapon.Operation.Set(this);

            OnBegin?.Invoke();
        }

        public event Action OnStop;
        public virtual void Stop()
        {
            Weapon.Operation.Clear(this);

            OnStop?.Invoke();
        }
	}
}