using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class WeaponPenetrationDamageModifier : WeaponDamage.Modifier
    {
        public override int Order => 200;

        public override void Sample(ref Damage.Request request, Damage.IDamagable target, WeaponHit.Data hit)
        {
            request.Value *= hit.Power;
        }
    }
}