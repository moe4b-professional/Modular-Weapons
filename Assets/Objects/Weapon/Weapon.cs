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
    [RequireComponent(typeof(AudioSource))]
    public class Weapon : MonoBehaviour
    {
        public WeaponConstraint Constraint { get; protected set; }
        public WeaponAction Action { get; protected set; }
        public WeaponHit Hit { get; protected set; }
        public WeaponDamage Damage { get; protected set; }
        public WeaponOperation Operation { get; protected set; }
        public WeaponActivation Activation { get; protected set; }
        public WeaponPivot Pivot { get; protected set; }
        public WeaponEffects Effects { get; protected set; }
        public WeaponMesh Mesh { get; protected set; }

        public class Behaviour : MonoBehaviour, IBehaviour<Weapon>
        {
            new public bool enabled
            {
                get => base.enabled && gameObject.activeInHierarchy;
                set => base.enabled = value;
            }

            //To force the enabled tick box on the component to show
            protected virtual void Start() { }
        }
        public Behaviours.Collection<Weapon> Behaviours { get; protected set; }
        
        public abstract class BaseModule<TReference> : Behaviour, IModule<TReference>
        {
            public abstract Weapon Weapon { get; }

            public TReference Reference { get; protected set; }
            public virtual void Setup(TReference reference)
            {
                this.Reference = reference;
            }

            public IOwner Owner => Weapon.Owner;

            public virtual void Configure()
            {

            }

            public virtual void Init()
            {

            }

            public virtual TType GetProcessor<TType>()
                where TType : class
            {
                var instance = Owner.GetProcessor<TType>();

                if (instance == null)
                    ExecuteProcessorError<TType>();

                return instance;
            }
            public void ExecuteProcessorError<TType>()
            {
                var message = "Module: " + GetType().Name + " Requires a processor of type: " + typeof(TType).FullName + " To function";

                var exception = new InvalidOperationException(message);

                throw exception;
            }

            public void ExecuteDependancyError<TType>()
            {
                var exception = Dependancy.CreateException<TType>(this);

                throw exception;
            }
        }
        public abstract class Module : BaseModule<Weapon>
        {
            public override Weapon Weapon => Reference;
        }
        public Modules.Collection<Weapon> Modules { get; protected set; }

        public AudioSource AudioSource { get; protected set; }
        
        public IOwner Owner { get; protected set; }
        public virtual void Setup(IOwner owner)
        {
            this.Owner = owner;

            Configure();

            Init();
        }
        public interface IOwner
        {
            GameObject Root { get; }

            Damage.IDamager Damager { get; }

            TType GetProcessor<TType>() where TType : class;
        }

        protected virtual void Configure()
        {
            AudioSource = GetComponent<AudioSource>();

            Behaviours = new Behaviours.Collection<Weapon>(this);
            Behaviours.Register(gameObject);

            Modules = new Modules.Collection<Weapon>(this);
            Modules.Register(Behaviours);

            Constraint = Modules.Depend<WeaponConstraint>();
            Action = Modules.Depend<WeaponAction>();
            Damage = Modules.Depend<WeaponDamage>();
            Hit = Modules.Depend<WeaponHit>();
            Operation = Modules.Depend<WeaponOperation>();
            Activation = Modules.Depend<WeaponActivation>();
            Pivot = Modules.Depend<WeaponPivot>();
            Effects = Modules.Depend<WeaponEffects>();
            Mesh = Modules.Depend<WeaponMesh>();

            Modules.Configure();
        }

        protected virtual void Init()
        {
            Modules.Init();
        }

        public delegate void ProcessDelegate();
        public event ProcessDelegate OnProcess;
        public virtual void Process()
        {
            LatePerformCondition = true;

            OnProcess?.Invoke();
        }

        protected virtual void LateUpdate()
        {
            if (LatePerformCondition)
            {
                LateProcess();
                LatePerformCondition = false;
            }
        }
        bool LatePerformCondition;
        public event ProcessDelegate OnLateProcess;
        protected virtual void LateProcess()
        {
            OnLateProcess?.Invoke();
        }
        
        public virtual void Equip() => Activation.Enable();
        public virtual void UnEquip() => Activation.Disable();
    }
}