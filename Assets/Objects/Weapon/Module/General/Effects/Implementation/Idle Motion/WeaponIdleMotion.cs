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

using MB;

namespace Game
{
	public class WeaponIdleMotion : Weapon.Module, WeaponEffects.IInterface
	{
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        public Transform Context => anchor.transform;

        public Modifier.Scale Scale { get; protected set; }

        [SerializeField]
        protected float speed = 0.4f;
        public float Speed { get { return speed; } }

        public Vector3 Target { get; protected set; }

        public Modules<WeaponIdleMotion> Modules { get; protected set; }
        public class Module : Weapon.Behaviour, IModule<WeaponIdleMotion>
        {
            public WeaponIdleMotion IdleMotion { get; protected set; }
            public virtual void Set(WeaponIdleMotion value) => IdleMotion = value;

            public Weapon Weapon => IdleMotion.Weapon;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponIdleMotion>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Weapon.OnProcess += Process;
        }

        void Process()
        {
            CalculateTarget();
        }

        protected virtual void CalculateTarget()
        {
            Target = Vector3.zero;

            Target += Vector3.right * Mathf.Sin(speed * Time.time);
            Target += Vector3.up * Mathf.Sin(speed * 2f * Time.time);
            Target += Vector3.forward * Mathf.Sin(speed * 3f * Time.time);
        }

        public abstract class Effect : Module
        {
            public Vector3 Offset { get; protected set; }

            public Transform Context => IdleMotion.Context;

            public override void Init()
            {
                base.Init();

                Weapon.OnProcess += Process;

                IdleMotion.Anchor.OnWriteDefaults += Write;
            }

            void Process() => CalculateOffset();
            protected abstract void CalculateOffset();

            protected abstract void Write();
        }
    }
}