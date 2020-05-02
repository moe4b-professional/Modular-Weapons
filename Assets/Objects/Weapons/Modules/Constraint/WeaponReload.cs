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
	public abstract class BaseWeaponReload : Weapon.Module, Weapon.IConstraint, WeaponOperation.IInterface
	{
        public bool IsProcessing => Weapon.Operation == this;
        public bool Active => IsProcessing;

        [SerializeField]
        protected bool auto = true;
        public bool Auto { get { return auto; } }

        public WeaponAmmo Ammo { get; protected set; }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            Ammo = Weapon.GetComponentInChildren<WeaponAmmo>();
        }

        public override void Init()
        {
            base.Init();

            if(Ammo == null)
            {
                Debug.LogError(FormatDependancyError<WeaponAmmo>());
                enabled = false;
                return;
            }

            Ammo.OnConsumption += ConsumptionCallback;

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData)
                Process(data as IData);
        }
        void Process(IData data)
        {
            if (data.Input)
            {
                if (CanPerform)
                    Perform();
            }
        }

        void ConsumptionCallback()
        {
            if (auto && Ammo.CanConsume == false && CanPerform)
                Perform();
        }

        public bool CanPerform
        {
            get
            {
                if (Ammo.Magazine.IsFull) return false;

                if (Ammo.Reserve.IsEmpty) return false;

                if (IsProcessing) return false;

                return true;
            }
        }
        public virtual void Perform()
        {
            Weapon.Operation.Set(this);
        }

        protected virtual void Complete()
        {
            Weapon.Operation.Clear();

            Ammo.Refill();
        }

        public virtual void Stop()
        {
            //TODO provide functionality to stop reload
        }

        public interface IData
        {
            bool Input { get; }
        }
    }

    public class WeaponReload : BaseWeaponReload
    {
        public override void Perform()
        {
            base.Perform();

            StartCoroutine(Procedure());
        }

        IEnumerator Procedure()
        {
            yield return new WaitForSeconds(1f);

            Complete();
        }
    }
}