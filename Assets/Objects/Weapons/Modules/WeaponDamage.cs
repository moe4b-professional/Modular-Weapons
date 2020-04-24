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
	public abstract class BaseWeaponDamage : Weapon.Module
	{
        public abstract Damage.Method SampleDamageMethod(Damage.IDamagable target);

        public abstract float SampleDamageValue(Damage.IDamagable target);

        public virtual Damage.Result Do(Damage.IDamagable target)
        {
            var value = SampleDamageValue(target);
            var method = SampleDamageMethod(target);

            var result = Owner.DoDamage(target, value, method);

            return result;
        }

        public virtual Damage.Result? Do(GameObject target)
        {
            var damagable = target.GetComponent<Damage.IDamagable>();

            if (damagable == null) return null;

            return Do(damagable);
        }
    }

    public class WeaponDamage : BaseWeaponDamage
    {
        [SerializeField]
        protected Damage.Method method = Damage.Method.Contact;
        public Damage.Method Method { get { return method; } }
        public override Damage.Method SampleDamageMethod(Damage.IDamagable target) => method;

        [SerializeField]
        protected float value = 25f;
        public float Value { get { return value; } }
        public override float SampleDamageValue(Damage.IDamagable target) => value;
    }
}