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
        public WeaponHit Hit { get; protected set; }

        public WeaponDamage Damage { get; protected set; }

		public class Module : Module<Weapon>
        {
            new public bool enabled
            {
                get => base.enabled && gameObject.activeInHierarchy;
                set => base.enabled = value;
            }

            public Weapon Weapon => Reference;

            public IOwner Owner => Weapon.Owner;

            //To force the enabled tick box on the component to show
            protected virtual void Start()
            {

            }

            public string FormatDependancyError<TDependancy>()
            {
                return "Module: " + GetType().Name + " Requires a module of type: " + typeof(TDependancy).Name + " To function";
            }
        }

        public Animator Animator { get; protected set; }

        public AudioSource AudioSource { get; protected set; }

        public IList<IConstraint> Constraints { get; protected set; }
        public interface IConstraint
        {
            bool Active { get; }
        }
        public bool HasActiveConstraints
        {
            get
            {
                for (int i = 0; i < Constraints.Count; i++)
                    if (Constraints[i].Active)
                        return true;

                return false;
            }
        }

        public IOwner Owner { get; protected set; }
        public virtual void Setup(IOwner owner)
        {
            this.Owner = owner;

            Configure();

            Init();
        }

        protected virtual void Configure()
        {
            Animator = GetComponentInChildren<Animator>();

            AudioSource = GetComponentInChildren<AudioSource>();

            Damage = GetComponentInChildren<WeaponDamage>();

            Hit = GetComponentInChildren<WeaponHit>();

            Modules.Configure(this);

            Constraints = GetComponentsInChildren<IConstraint>();
        }

        protected virtual void Init()
        {
            Modules.Init(this);
        }

        public delegate void ProcessDelegate(IProcessData data);
        public event ProcessDelegate OnProcess;
        public virtual void Process(IProcessData data)
        {
            LateProcessData = data;

            OnProcess?.Invoke(data);

            if(data.Input)
            {
                if(HasActiveConstraints)
                {

                }
                else
                {
                    Action();
                }
            }
        }

        protected virtual void LateUpdate()
        {
            if(LateProcessData != null)
            {
                LateProcess(LateProcessData);
                LateProcessData = null;
            }
        }
        public IProcessData LateProcessData { get; protected set; }
        public event ProcessDelegate OnLateProcess;
        protected virtual void LateProcess(IProcessData data)
        {
            OnLateProcess?.Invoke(data);
        }

        public interface IProcessData
        {
            bool Input { get; }
        }

        public interface IOwner
        {
            GameObject gameObject { get; }

            Damage.IDamager Damager { get; }
        }

        public interface IEffect
        {
            float Scale { get; set; }
        }

        public delegate void ActionDelegate();
        public event ActionDelegate OnAction;
        protected virtual void Action()
        {
            OnAction?.Invoke();
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