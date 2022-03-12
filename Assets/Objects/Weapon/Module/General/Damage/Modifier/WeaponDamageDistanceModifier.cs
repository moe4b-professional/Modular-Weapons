using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

namespace Game
{
    public class WeaponDamageDistanceModifier : WeaponDamage.Modifier
    {
        [SerializeField]
        AnimationCurve falloff;
        public AnimationCurve Falloff => falloff;

        public override int Order => 200;

        public override void Sample(ref Damage.Request request, Damage.IDamagable target, WeaponHit.Data hit)
        {
            request.Value *= falloff.Evaluate(hit.Distance);
        }

        public WeaponDamageDistanceModifier()
        {
            falloff = new AnimationCurve()
            {
                keys = new Keyframe[]
                {
                    new Keyframe(0f, 1f),
                    new Keyframe(1f, 1f),
                },
                postWrapMode = WrapMode.ClampForever,
                preWrapMode = WrapMode.ClampForever,
            };
        }
    }
}
