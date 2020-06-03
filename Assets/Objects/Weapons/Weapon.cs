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

        public References.Collection<Weapon> Modules { get; protected set; }

        public abstract class BaseModule<TReference> : MonoBehaviour, IReference<TReference>
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

            public virtual void Configure(TReference reference)
            {
                this.Reference = reference;
            }

            public virtual void Init()
            {
                
            }

            public virtual TProcessor GetProcessor<TProcessor>()
                where TProcessor : class
            {
                var instance = Weapon.Processor.GetDependancy<TProcessor>();

                if (instance == null)
                    ExecuteProcessorError<TProcessor>();

                return instance;
            }

            public void ExecuteDependancyError<TDependancy>()
            {
                var message = "Module: " + GetType().Name + " Requires a module of type: " + typeof(TDependancy).Name + " To function";

                Debug.LogError(message, gameObject);
                enabled = false;
            }
            public void ExecuteProcessorError<TProcessor>()
            {
                var message = "Module: " + GetType().Name + " Requires a processor of type: " + typeof(TProcessor).FullName + " To function";

                Debug.LogError(message, gameObject);
                enabled = false;
            }
        }
        public abstract class Module : BaseModule<Weapon>
        {
            public override Weapon Weapon => Reference;
        }

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

        public IProcessor Processor => Owner.Processor;
        public interface IProcessor
        {
            bool Input { get; }

            T GetDependancy<T>() where T : class;
        }

        protected virtual void Configure()
        {
            AudioSource = GetComponent<AudioSource>();

            Modules = new References.Collection<Weapon>(this);

            Constraint = FindModule<WeaponConstraint>();
            Action = FindModule<WeaponAction>();
            Damage = FindModule<WeaponDamage>();
            Hit = FindModule<WeaponHit>();
            Operation = FindModule<WeaponOperation>();
            Activation = FindModule<WeaponActivation>();
            Pivot = FindModule<WeaponPivot>();
            Effects = FindModule<WeaponEffects>();
            Mesh = FindModule<WeaponMesh>();

            TModule FindModule<TModule>()
                where TModule : class
            {
                var instance = Modules.Find<TModule>();

                if (instance == null)
                    throw new Exception("Weapon " + name + " Requires a " + typeof(TModule).Name + " Component");

                return instance;
            }

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