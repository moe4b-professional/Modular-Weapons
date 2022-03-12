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
    public class WeaponDamageConstantModifier : WeaponDamage.Modifier
    {
        public override int Order => -200;

        [SerializeField]
        protected float value = 25f;
        public float Value { get { return value; } }

        public override void Sample(ref Damage.Request request, Damage.IDamagable target, WeaponHit.Data hit)
        {
            request.Value += value;
        }
    }
}