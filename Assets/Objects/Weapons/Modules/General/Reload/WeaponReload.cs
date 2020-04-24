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
	public abstract class BaseWeaponReload : Weapon.Module
	{
        [SerializeField]
        protected bool auto;
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
                Debug.LogError(GetType().Name + " needs a " + nameof(WeaponAmmo) + " module to function, ignoring module");
                enabled = false;
                return;
            }

            Ammo.OnConsumption += ConsumptionCallback;
        }

        void ConsumptionCallback()
        {
            if (Ammo.Magazine.IsEmpty)
                if (auto && CanPerform)
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
        public bool IsProcessing { get; protected set; } = false;
        public virtual void Perform()
        {
            IsProcessing = true;
        }

        protected virtual void Complete()
        {
            IsProcessing = false;
            Ammo.Refill();
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