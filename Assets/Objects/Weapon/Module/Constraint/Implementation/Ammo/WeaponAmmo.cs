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

using MB;

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
                    this.value = value;
                }
            }

            public bool IsEmpty => value == 0;

            [SerializeField]
            protected int max;
            public int Max { get { return max; } }

            public bool IsFull => value >= max;

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
        protected int chamber = 1;
        public int Chamber => chamber;

        [SerializeField]
        protected int consumption = 1;
        public int Consumption { get { return consumption; } }

        public int RemainingUses => consumption == 0 ? int.MaxValue : magazine.Value / consumption;

        public Modules<WeaponAmmo> Modules { get; protected set; }
        public abstract class Module : Weapon.Behaviour, IModule<WeaponAmmo>
        {
            public WeaponAmmo Ammo { get; protected set; }
            public virtual void Set(WeaponAmmo value) => Ammo = value;

            public Weapon Weapon => Ammo.Weapon;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponAmmo>(this);
            Modules.Register(Weapon.Behaviours, SegmentScope.Global);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            if (magazine.Value == magazine.Max) magazine.Value += chamber;
        }

        public event Action OnConsumption;
        public virtual void Consume()
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
            if (magazine.Value > 0) requirement += chamber;

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