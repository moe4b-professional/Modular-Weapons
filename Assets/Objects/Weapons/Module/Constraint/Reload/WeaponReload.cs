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
	public abstract class WeaponReload : Weapon.Module, WeaponConstraint.IInterface, WeaponOperation.IInterface
	{
        public bool IsProcessing => Equals(Weapon.Operation.Value);
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

        public class Module : Weapon.Module<WeaponReload>
        {
            public WeaponReload Reload => Reference;

            public override Weapon Weapon => Reload.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Ammo = Dependancy.Get<WeaponAmmo>(Weapon.gameObject);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            if(Ammo == null)
            {
                ExecuteDependancyError<WeaponAmmo>();
                return;
            }

            Weapon.OnLateProcess += LateProcess;

            Weapon.Activation.OnDisable += DisableCallback;

            References.Init(this);
        }

        void DisableCallback()
        {
            if (IsProcessing) Stop();
        }

        void LateProcess(Weapon.IProcessData data)
        {
            if (data is IData)
                LateProcess(data as IData);
        }
        void LateProcess(IData data)
        {
            if (data.Input)
            {
                if (CanPerform)
                    Perform();
            }
        }

        public virtual void Perform()
        {
            Weapon.Operation.Set(this);
        }

        public event Action OnComplete;
        protected virtual void Complete()
        {
            Stop();

            OnComplete?.Invoke();
        }

        public virtual void Stop()
        {
            //TODO provide functionality to stop reload

            Weapon.Operation.Clear();
        }

        public interface IData
        {
            bool Input { get; }
        }
    }
}