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
	public class WeaponIdleMotion : Weapon.Module, WeaponEffects.IInterface
	{
        public Modifier.Scale Scale { get; protected set; }

        [SerializeField]
        protected float speed = 1.2f;
        public float Speed { get { return speed; } }

        public Vector3 Target { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;
        public Transform Context => Pivot.transform;

        public class Module : Weapon.BaseModule<WeaponIdleMotion>
        {
            public WeaponIdleMotion IdleMotion => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }

        public Modules.Collection<WeaponIdleMotion> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();

            Modules = new Modules.Collection<WeaponIdleMotion>(this);

            Modules.Register(Weapon.Behaviours);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Pivot.OnProcess += Process;

            Modules.Init();
        }

        public event Action OnProcess;
        void Process()
        {
            CalculateTarget();

            OnProcess?.Invoke();
        }

        protected virtual void CalculateTarget()
        {
            Target = Vector3.zero;

            Target += Vector3.right * Mathf.Sin(speed * Time.time);
            Target += Vector3.up * Mathf.Sin(speed * 2f * Time.time);
            Target += Vector3.forward * Mathf.Sin(speed * 3f * Time.time);
        }
    }
}