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
        public abstract Damage.Request SampleRequest(Damage.IDamagable target, WeaponHit.Data hit);

        public override void Init()
        {
            base.Init();

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
            var request = SampleRequest(target, hit);

            var result = Owner.Damager.Perform(target, request);

            return result;
        }
    }
}