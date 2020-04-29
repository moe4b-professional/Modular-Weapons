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
            public Weapon Weapon => Reference;

            public IOwner Owner => Weapon.Owner;

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

        public interface IProcessData
        {
            bool Input { get; }
        }

        public interface IOwner
        {
            GameObject gameObject { get; }

            Damage.IDamager Damager { get; }
        }

        public delegate void ActionDelegate();
        public event ActionDelegate OnAction;
        protected virtual void Action()
        {
            OnAction?.Invoke();
        }
    }
}