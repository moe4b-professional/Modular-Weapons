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
    public abstract class WeaponDamage : Weapon.Module
    {
        public abstract Damage.Method SampleDamageMethod(Damage.IDamagable target, WeaponHit.Data hit);

        public abstract float SampleDamageValue(Damage.IDamagable target, WeaponHit.Data hit);

        public virtual Damage.Result Do(Damage.IDamagable target, WeaponHit.Data hit)
        {
            var value = SampleDamageValue(target, hit);
            var method = SampleDamageMethod(target, hit);

            var request = new Damage.Request(value, method);

            var result = Owner.Damager.DoDamage(target, request);

            return result;
        }
    }
}