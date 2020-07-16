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
	public class WeaponTug : Weapon.Module, WeaponEffects.IInterface
	{
        [SerializeField]
        protected float range = 0.004f;
        public float Range { get { return range; } }

        [SerializeField]
        protected float speed = 10f;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float max = 3f;
        public float Max { get { return max; } }

        public Modifier.Scale Scale { get; protected set; }

        public float Target { get; protected set; }
        public float Value { get; protected set; }

        public IProcessor Processor { get; protected set; }
		public interface IProcessor
        {
            Vector3 PlanarVelocity { get; }
        }

        public WeaponPivot Pivot => Weapon.Pivot;
        public Transform Context => Pivot.transform;

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Scale = new Modifier.Scale();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Pivot.OnProcess += Process;
        }

        void Process()
        {
            CalculateTarget();

            Value = Mathf.Lerp(Value, Target, speed * Time.deltaTime);

            Context.position -= Weapon.transform.forward * Value;
        }

        protected virtual void CalculateTarget()
        {
            Target = Processor.PlanarVelocity.magnitude;

            Target = Mathf.Clamp(Target, 0f, max);

            Target *= range * Scale.Value;
        }
    }
}