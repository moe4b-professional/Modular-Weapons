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
	public class WeaponTug : Weapon.Module, WeaponEffects.IInterface
	{
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        public Transform Context => anchor.transform;

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
		public interface IProcessor : Weapon.IProcessor
        {
            Vector3 PlanarVelocity { get; }
        }
        
        public override void Configure()
        {
            base.Configure();

            Processor = Weapon.GetProcessor<IProcessor>(this);

            Scale = new Modifier.Scale();
        }

        public override void Init()
        {
            base.Init();

            Weapon.Effects.Register(this);

            Weapon.OnProcess += Process;

            anchor.OnWriteDefaults += Write;
        }

        void Process()
        {
            CalculateTarget();

            Value = Mathf.Lerp(Value, Target, speed * Time.deltaTime);
        }

        protected virtual void CalculateTarget()
        {
            Target = Processor.PlanarVelocity.magnitude;

            Target = Mathf.Clamp(Target, 0f, max);

            Target *= range * Scale.Value;
        }

        void Write()
        {
            Context.position -= Weapon.transform.forward * Value;
        }
    }
}