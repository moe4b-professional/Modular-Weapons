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
    public class WeaponActionMode : Weapon.Module
    {
        public IList<IState> List { get; protected set; }
        public interface IState
        {
            bool enabled { get; set; }
        }

        public virtual void Set(int index)
        {
            for (int i = 0; i < List.Count; i++)
                List[i].enabled = i == index;
        }

        public int Index { get; protected set; }

        public class Module : Weapon.Module<WeaponActionMode>
        {
            public WeaponActionMode Mode => Reference;

            public override Weapon Weapon => Mode.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            List = Dependancy.GetAll<IState>(gameObject);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;

            Index = 0;

            for (int i = 0; i < List.Count; i++)
            {
                if(List[i].enabled)
                {
                    Index = i;
                    break;
                }
            }

            if(List.Count > 0) Set(Index);

            References.Init(this);
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData) Process(data as IData);
        }
        void Process(IData data)
        {
            if(data.Switch)
            {
                if (CanChange)
                    Change();
            }
        }

        public virtual bool CanChange
        {
            get
            {
                if (Weapon.Constraint.Active) return false;

                if (enabled == false) return false;

                if (List.Count < 2) return false;

                return true;
            }
        }

        public delegate void ChangeDelegate(int index, IState module);
        public event ChangeDelegate OnChange;
        void Change()
        {
            Index++;

            if (Index >= List.Count) Index = 0;

            Set(Index);

            OnChange?.Invoke(Index, List[Index]);
        }

        public interface IData
        {
            bool Switch { get; }
        }
    }
}