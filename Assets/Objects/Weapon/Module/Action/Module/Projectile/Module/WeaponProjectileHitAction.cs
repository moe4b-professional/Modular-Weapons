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
	public class WeaponProjectileHitAction : WeaponProjectileAction.Module
	{
        public override void Init()
        {
            base.Init();

            Action.OnPerform += ActionCallback;
        }

        void ActionCallback(Projectile projectile)
        {
            projectile.OnHit += ProjectileHitCallback;

            projectile.OnDestroy += ProjectileDestroyCallback;
        }

        void ProjectileHitCallback(Projectile projectile, WeaponHit.Data data)
        {
            Weapon.Hit.Process(data);
        }

        void ProjectileDestroyCallback(Projectile projectile)
        {
            projectile.OnHit -= ProjectileHitCallback;

            projectile.OnDestroy -= ProjectileDestroyCallback;
        }
    }
}