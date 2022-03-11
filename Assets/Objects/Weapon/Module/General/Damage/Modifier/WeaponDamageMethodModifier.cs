using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WeaponDamageMethodModifier : WeaponDamage.Modifier
    {
        public override int Order => 0;

        [SerializeField]
        Damage.Method method;
        public Damage.Method Method => method;

        public override void Sample(ref Damage.Request request, Damage.IDamagable target, WeaponHit.Data hit)
        {
            request.Method = method;
        }
    }
}
