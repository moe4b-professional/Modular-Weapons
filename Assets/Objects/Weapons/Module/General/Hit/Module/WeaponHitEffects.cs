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
	public class WeaponHitEffects : WeaponHit.Module
	{
        [SerializeField]
        protected SurfaceHitEffectPack pack;
        public SurfaceHitEffectPack Pack { get { return pack; } }

        public override void Init()
        {
            base.Init();

            Hit.OnProcess += Process;
        }

        void Process(WeaponHit.Data hit)
        {
            var surface = Surface.Get(hit.Collider);

            var effect = surface == null ? pack.Default : pack.Find(surface.Material);

            var instance = Spawn(effect, hit);
        }

        protected virtual GameObject Spawn(SurfaceHitEffect effect, WeaponHit.Data data)
        {
            return effect.Spawn(data.Contact.Point, data.Contact.Normal, data.Collider.transform);
        }
    }
}