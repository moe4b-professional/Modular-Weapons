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
	public class WeaponReload : Weapon.Module, WeaponConstraint.IInterface, WeaponOperation.IInterface
	{
        public bool IsProcessing => Weapon.Operation.Is(this);

        public bool Active => IsProcessing;

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

        public Modules<WeaponReload> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponReload>
        {
            public WeaponReload Reload { get; protected set; }
            public virtual void Set(WeaponReload value) => Reload = value;

            public Weapon Weapon => Reload.Weapon;
        }

        public abstract class Procedure : Module
        {

        }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            bool Input { get; }
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponReload>(this);
            Modules.Register(Weapon.Behaviours);

            Ammo = Weapon.Modules.Depend<WeaponAmmo>();

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);
        }

        public override void Init()
        {
            base.Init();
            
            Weapon.OnProcess += Process;

            Weapon.Activation.OnDisable += DisableCallback;
        }

        void DisableCallback()
        {
            if (IsProcessing) Stop();
        }

        void Process()
        {
            if (Processor.Input)
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
    }
}