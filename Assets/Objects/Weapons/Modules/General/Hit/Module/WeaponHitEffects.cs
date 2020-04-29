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
	public class WeaponHitEffects : Weapon.Module
	{
        [SerializeField]
        protected SurfaceHitEffectPack pack;
        public SurfaceHitEffectPack Pack { get { return pack; } }

        public override void Init()
        {
            base.Init();

            Weapon.Hit.OnProcess += Process;
        }

        void Process(HitData hit)
        {
            var surface = GetSurface(hit);

            var effect = surface == null ? pack.Default : pack.Find(surface.Material);

            var instance = effect.Spawn(hit);
        }

        public static Surface GetSurface(HitData hit)
        {
            Surface surface;

            surface = hit.Collider.GetComponent<Surface>();
            if (surface != null) return surface;

            surface = hit.GameObject.GetComponent<Surface>();
            if (surface != null) return surface;

            return null;
        }
    }
}