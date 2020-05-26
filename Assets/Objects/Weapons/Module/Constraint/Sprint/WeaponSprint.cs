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

        public bool Active => Equals(Weapon.Operation.Value);

        bool WeaponConstraint.IInterface.Constraint => Active;

        public class Module : Weapon.Module<WeaponSprint>
        {
            public WeaponSprint Sprint => Reference;

            public override Weapon Weapon => Sprint.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;

            References.Init(this);
        }

        void DisableCallback()
        {
            if (Active) Stop();
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData) Process(data as IData);
        }
        void Process(IData data)
        {
            if (data.Weight > minWeight)
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

        public interface IData
        {
            float Weight { get; }
        }
	}
}