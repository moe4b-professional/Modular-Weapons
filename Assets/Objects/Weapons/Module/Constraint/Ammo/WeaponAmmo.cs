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
    public class WeaponAmmo : Weapon.Module
    {
        public bool CanConsume => magazine.Value >= consumption;

        [SerializeField]
        protected MaxValue magazine = new MaxValue(30);
        public MaxValue Magazine { get { return magazine; } }

        [SerializeField]
        protected MaxValue reserve = new MaxValue(120);
        public MaxValue Reserve { get { return reserve; } }

        [Serializable]
        public class MaxValue
        {
            [SerializeField]
            protected int value;
            public int Value
            {
                get
                {
                    return value;
                }
                set
                {
                    if(value > max)
                    {
                        value = max;
                        Debug.LogWarning("Cannot set value bigger than a max of " + max);
                    }

                    this.value = value;
                }
            }

            public bool IsEmpty => value == 0;

            [SerializeField]
            protected int max;
            public int Max { get { return max; } }

            public bool IsFull => value == max;

            public MaxValue(int value, int max)
            {
                this.value = value;

                this.max = max;
            }
            public MaxValue(int value) : this(value, value)
            {

            }
        }

        [SerializeField]
        protected int consumption = 1;
        public int Consumption { get { return consumption; } }

        public class Module : Weapon.Module<WeaponAmmo>
        {
            public WeaponAmmo Ammo => Reference;

            public override Weapon Weapon => Ammo.Weapon;
        }

        public override void Configure(Weapon reference)
        {
            base.Configure(reference);

            References.Configure(this);
        }

        public override void Init()
        {
            base.Init();

            Weapon.Action.OnLatePerform += Action;

            References.Init(this);
        }

        void Action()
        {
            if (CanConsume) Consume();
        }

        public event Action OnConsumption;
        protected virtual void Consume()
        {
            magazine.Value -= consumption;

            OnConsumption?.Invoke();
        }

        public event Action OnRefill;
        public virtual bool Refill()
        {
            if (magazine.IsFull) return false;
            if (reserve.IsEmpty) return false;

            var requirement = magazine.Max - magazine.Value;

            if(reserve.Value >= requirement)
            {
                reserve.Value -= requirement;
                magazine.Value += requirement;
            }
            else
            {
                magazine.Value += reserve.Value;
                reserve.Value = 0;
            }

            OnRefill?.Invoke();

            return true;
        }
    }
}