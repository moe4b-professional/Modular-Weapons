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
	public class WeaponSprint : Weapon.Module, WeaponOperation.IInterface, WeaponConstraint.IInterface
	{
        [SerializeField]
        protected float minWeight = 0.5f;
        public float MinWeight { get { return minWeight; } }

        public bool Active => Weapon.Operation.Is(this);

        bool WeaponConstraint.IInterface.Constraint => Active;

        public abstract class Module : Weapon.BaseModule<WeaponSprint>
        {
            public WeaponSprint Sprint => Reference;

            public override Weapon Weapon => Sprint.Weapon;
        }

        public Modules.Collection<WeaponSprint> Modules { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            float Weight { get; }

            bool Active { get; }
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Modules = new Modules.Collection<WeaponSprint>(this, Weapon.gameObject);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;

            Modules.Init();
        }

        void DisableCallback()
        {
            if (Active) Stop();
        }

        void Process()
        {
            if (Processor.Active && Processor.Weight > minWeight)
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