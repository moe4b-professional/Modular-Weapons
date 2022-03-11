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
    public class WeaponDamage : Weapon.Module
    {
        [field: SerializeField, DebugOnly]
        public List<Modifier> Modifiers { get; private set; }
        public abstract class Modifier : Module
        {
            public abstract int Order { get; }

            public abstract void Sample(ref Damage.Request request, Damage.IDamagable target, WeaponHit.Data hit);

            public static int Sort(Modifier x, Modifier y) => x.Order.CompareTo(y.Order);
        }

        [field: SerializeField, DebugOnly]
        public Modules<WeaponDamage> Modules { get; private set; }
        public class Module : Weapon.Behaviour, IModule<WeaponDamage>
        {
            [field: SerializeField, DebugOnly]
            public WeaponDamage Damage { get; private set; }

            public virtual void Set(WeaponDamage reference)
            {
                Damage = reference;
            }
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponDamage>(this);
            Modules.Register(Weapon.Behaviours);

            Modifiers = Modules.FindAll<Modifier>();
            Modifiers.Sort(Modifier.Sort);

            Modules.Set();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.Hit.OnProcess += HitCallback;
        }

        void HitCallback(WeaponHit.Data data)
        {
            var damagable = data.GameObject.GetComponent<Damage.IDamagable>();

            if (damagable != null)
                Perform(damagable, data);
        }

        public virtual Damage.Result Perform(Damage.IDamagable target, WeaponHit.Data hit)
        {
            var request = new Damage.Request();

            for (int i = 0; i < Modifiers.Count; i++)
                Modifiers[i].Sample(ref request, target, hit);

            var result = Owner.Damager.Perform(target, request);

            return result;
        }
    }
}