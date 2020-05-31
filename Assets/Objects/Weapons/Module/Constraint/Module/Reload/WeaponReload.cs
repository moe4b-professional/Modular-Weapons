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
	public class WeaponReload : Weapon.Module<WeaponReload.IProcessor>, WeaponConstraint.IInterface, WeaponOperation.IInterface
	{
        public bool IsProcessing => Weapon.Operation.Is(this);
        bool WeaponConstraint.IInterface.Constraint => IsProcessing;

        public WeaponAmmo Ammo { get; protected set; }

        public bool CanPerform
        {
            get
            {
                if (Ammo.Magazine.IsFull) return false;

                if (Ammo.Reserve.IsEmpty) return false;

                if (IsProcessing) return false;

                if (Weapon.Constraint.Active) return false;

                return true;
            }
        }

        public class Module : Weapon.BaseModule<WeaponReload>
        {
            public WeaponReload Reload => Reference;

            public override Weapon Weapon => Reload.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Ammo = Dependancy.Get<WeaponAmmo>(Weapon.gameObject);

            References.Configure(this, Weapon.gameObject);
        }

        public override void Init()
        {
            base.Init();

            if(Ammo == null)
            {
                ExecuteDependancyError<WeaponAmmo>();
                return;
            }

            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;

            References.Init(this, Weapon.gameObject);
        }

        void DisableCallback()
        {
            if (IsProcessing) Stop();
        }

        void Process()
        {
            if (HasProcessor) Process(Processor);
        }
        void Process(IProcessor data)
        {
            if (data.Input)
            {
                if (CanPerform)
                    Perform();
            }
        }

        public event Action OnPerform;
        public virtual void Perform()
        {
            Weapon.Operation.Set(this);

            OnPerform?.Invoke();
        }

        public event Action OnRefill;
        public virtual void Refill()
        {
            Ammo.Refill();

            OnRefill?.Invoke();
        }

        public event Action OnComplete;
        public virtual void Complete()
        {
            Stop();

            OnComplete?.Invoke();
        }
        
        public virtual void Stop()
        {
            //TODO provide functionality to stop reload

            Weapon.Operation.Clear();
        }

        public interface IProcessor
        {
            bool Input { get; }
        }
    }
}