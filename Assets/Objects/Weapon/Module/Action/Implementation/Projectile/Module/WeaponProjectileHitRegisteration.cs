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
	public class WeaponProjectileHitRegisteration : WeaponProjectileAction.Module
	{
        public override void Init()
        {
            base.Init();

            Action.OnPerform += ActionCallback;
        }

        void ActionCallback(Projectile projectile)
        {
            projectile.OnHit += HitCallback;

            projectile.OnDestroy += DestoryCallback;
        }

        void HitCallback(Projectile projectile, WeaponHit.Data data)
        {
            Weapon.Hit.Process(data);
        }

        void DestoryCallback(Projectile projectile)
        {
            projectile.OnHit -= HitCallback;

            projectile.OnDestroy -= DestoryCallback;
        }
    }
}