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
		public class Module : Module<Weapon>
        {
            public Weapon Weapon => Reference;

            public Entity Owner => Weapon.Owner;
        }

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

        public Entity Owner { get; protected set; }
        public virtual void Setup(Entity owner)
        {
            this.Owner = owner;

            Configure();

            Init();
        }

        protected virtual void Configure()
        {
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

            if(data.PrimaryInput)
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
            bool PrimaryInput { get; }
        }
        public struct ProcessData : IProcessData
        {
            public bool PrimaryInput { get; private set; }

            public ProcessData(bool input)
            {
                this.PrimaryInput = input;
            }
        }

        public delegate void ActionDelegate();
        public event ActionDelegate OnAction;
        protected virtual void Action()
        {
            OnAction?.Invoke();
        }
    }
}