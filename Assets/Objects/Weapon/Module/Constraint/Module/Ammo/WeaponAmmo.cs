﻿using System;
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

        public int RemainingUses => consumption == 0 ? int.MaxValue : magazine.Value / consumption;

        public abstract class Module : Weapon.BaseModule<WeaponAmmo>
        {
            public WeaponAmmo Ammo => Reference;

            public override Weapon Weapon => Ammo.Weapon;
        }

        public Modules.Collection<WeaponAmmo> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<WeaponAmmo>(this);
            Modules.Register(Weapon.Behaviours, ReferenceCollection.Scope.All);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
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