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
    public class WeaponConstantDamage : WeaponDamage
    {
        [SerializeField]
        protected Damage.Method method = Damage.Method.Projectile;
        public Damage.Method Method { get { return method; } }

        [SerializeField]
        protected float value = 25f;
        public float Value { get { return value; } }

        public override Damage.Request SampleRequest(Damage.IDamagable target, WeaponHit.Data hit)
        {
            return new Damage.Request(value * hit.Power, method);
        }
    }
}