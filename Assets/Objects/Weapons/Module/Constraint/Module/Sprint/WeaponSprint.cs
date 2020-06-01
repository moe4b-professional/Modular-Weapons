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
	public class WeaponSprint : Weapon.Module<WeaponSprint.IProcessor>, WeaponOperation.IInterface, WeaponConstraint.IInterface
	{
        [SerializeField]
        protected float minWeight = 0.5f;
        public float MinWeight { get { return minWeight; } }

        public bool Active => Weapon.Operation.Is(this);

        bool WeaponConstraint.IInterface.Constraint => Active;

        public abstract class Module : Weapon.BaseModule<WeaponSprint, IProcessor>
        {
            public WeaponSprint Sprint => Reference;

            public override Weapon Weapon => Sprint.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Modules.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;

            Modules.Init(this);
        }

        void DisableCallback()
        {
            if (Active) Stop();
        }

        void Process()
        {
            if (HasProcessor) Process(Processor);
        }
        void Process(IProcessor processor)
        {
            if (processor.Active && processor.Axis > minWeight)
            {
                if(Active == false)
                    Begin();
            }
            else
            {
                if(Active)
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

        public interface IProcessor
        {
            bool Active { get; }

            float Weight { get; }

            float Axis { get; }
        }
	}
}