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
        public abstract Damage.Method SampleDamageMethod(Damage.IDamagable target);

        public abstract float SampleDamageValue(Damage.IDamagable target);

        public virtual Damage.Result Do(Damage.IDamagable target)
        {
            var value = SampleDamageValue(target);
            var method = SampleDamageMethod(target);

            var request = new Damage.Request(value, method);

            var result = Owner.Damager.DoDamage(target, request);

            return result;
        }
    }
}