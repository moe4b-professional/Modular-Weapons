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
    public class Weapon : MonoBehaviour
    {
        public WeaponConstraint Constraint { get; protected set; }

        public WeaponAction Action { get; protected set; }

        public WeaponHit Hit { get; protected set; }

        public WeaponDamage Damage { get; protected set; }

        public WeaponOperation Operation { get; protected set; }

        public WeaponActivation Activation { get; protected set; }

        public WeaponEffects Effects { get; protected set; }

        public WeaponMesh Mesh { get; protected set; }

        public abstract class BaseModule<TReference, TProcessor> : MonoBehaviour, IReference<TReference>
            where TProcessor : class
        {
            new public bool enabled
            {
                get => base.enabled && gameObject.activeInHierarchy;
                set => base.enabled = value;
            }

            //To force the enabled tick box on the component to show
            protected virtual void Start() { }

            public TReference Reference { get; protected set; }

            public abstract Weapon Weapon { get; }

            public IOwner Owner => Weapon.Owner;

            public TProcessor Processor { get; protected set; }
            public bool HasProcessor => Processor != null;

            public virtual void Configure(TReference reference)
            {
                this.Reference = reference;

                Processor = Owner.Processor.GetDependancy<TProcessor>();
            }

            public virtual void Init()
            {
                
            }

            public string FormatDependancyError<TDependancy>()
            {
                return "Module: " + GetType().Name + " Requires a module of type: " + typeof(TDependancy).Name + " To function";
            }
            public void ExecuteDependancyError<TDependancy>()
            {
                Debug.LogError(FormatDependancyError<TDependancy>(), gameObject);
                enabled = false;
            }
        }
        public abstract class BaseModule<TReference> : BaseModule<TReference, IProcessor> { }

        public class Module<TProcessor> : BaseModule<Weapon, TProcessor>
            where TProcessor : class
        {
            public override Weapon Weapon => Reference;
        }
        public class Module : Module<IProcessor> { }

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
            GameObject gameObject { get; }

            Damage.IDamager Damager { get; }

            IProcessor Processor { get; }
        }

        protected virtual void Configure()
        {
            AudioSource = this.GetDependancy<AudioSource>();

            Constraint = this.GetDependancy<WeaponConstraint>();
            Action = this.GetDependancy<WeaponAction>();
            Damage = this.GetDependancy<WeaponDamage>();
            Hit = this.GetDependancy<WeaponHit>();
            Operation = this.GetDependancy<WeaponOperation>();
            Activation = this.GetDependancy<WeaponActivation>();
            Effects = this.GetDependancy<WeaponEffects>();
            Mesh = this.GetDependancy<WeaponMesh>();

            References.Configure(this);
        }

        protected virtual void Init()
        {
            References.Init(this);
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

        public interface IProcessor
        {
            bool Input { get; }

            T GetDependancy<T>() where T : class;
        }
    }

    [Serializable]
    public struct ValueRange
    {
        [SerializeField]
        float min;
        public float Min { get { return min; } }

        [SerializeField]
        float max;
        public float Max { get { return max; } }

        public float Random => UnityEngine.Random.Range(min, max);

        public float Lerp(float t) => Mathf.Lerp(min, max, t);

        public float Clamp(float value) => Mathf.Clamp(value, min, max);

        public ValueRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}