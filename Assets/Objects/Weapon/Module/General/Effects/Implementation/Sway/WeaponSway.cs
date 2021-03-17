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
	public class WeaponSway : Weapon.Module, WeaponEffects.IInterface
    {
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        [SerializeField]
        protected MultiplierData multiplier = new MultiplierData(0.3f, 0.2f);
        public MultiplierData Multiplier { get { return multiplier; } }
        [Serializable]
        public struct MultiplierData
        {
            [SerializeField]
            float look;
            public float Look { get { return look; } }

            [SerializeField]
            float move;
            public float Move { get { return move; } }

            public MultiplierData(float look, float move)
            {
                this.look = look;

                this.move = move;
            }
        }

        [SerializeField]
        protected SpeedData speed = new SpeedData(4, 5);
        public SpeedData Speed { get { return speed; } }
        [Serializable]
        public struct SpeedData
        {
            [SerializeField]
            float set;
            public float Set { get { return set; } }

            [SerializeField]
            float reset;
            public float Reset { get { return reset; } }

            public SpeedData(float set, float reset)
            {
                this.set = set;

                this.reset = reset;
            }
        }

        public Vector2 Target { get; protected set; }
        public Vector2 Value { get; protected set; }

        public Modifier.Scale Scale { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor : Weapon.IProcessor
        {
            Transform Axis { get; }

            Vector2 LookDelta { get; }

            Vector3 RelativeVelocity { get; }
        }

        public Modules<WeaponSway> Modules { get; protected set; }
        public class Module : Weapon.Behaviour, IModule<WeaponSway>
        {
            public WeaponSway Sway { get; protected set; }
            public virtual void Set(WeaponSway value) => Sway = value;

            public Weapon Weapon => Sway.Weapon;
        }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponSway>(this);
            Modules.Register(Weapon.Behaviours);

            Modules.Set();
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
        }

        void Process()
        {
            CalculateTarget();

            Value = Vector2.Lerp(Value, Target, speed.Set * Time.deltaTime);

            Target = Vector2.Lerp(Value, Vector2.zero, speed.Reset * Time.deltaTime);
        }

        protected virtual void CalculateTarget()
        {
            Target = Vector3.zero;

            if (enabled)
            {
                var unscaledLookDelta = Processor.LookDelta / Time.deltaTime / 100f;

                Target += Vector2.right * unscaledLookDelta.x * multiplier.Look;
                Target += Vector2.up * unscaledLookDelta.y * multiplier.Look;

                Target += Vector2.right * Processor.RelativeVelocity.x * multiplier.Move;

                Target = -Target;
            }
        }

        public abstract class Effect : Module
        {
            [SerializeField]
            protected float multiplier = 1f;
            public float Multiplier => multiplier;

            public abstract Vector3 Scale { get; }

            public Vector3 Offset { get; protected set; }

            public TransformAnchor Anchor => Sway.Anchor;

            public Transform Axis => Sway.Processor.Axis;

            protected virtual void Reset()
            {

            }

            public override void Configure()
            {
                base.Configure();

                Offset = Vector3.zero;
            }

            public override void Init()
            {
                base.Init();

                Weapon.OnProcess += Process;

                Sway.Anchor.OnWriteDefaults += Write;
            }

            protected virtual void Process()
            {
                CalculateOffset();
            }
            protected abstract void CalculateOffset();

            protected abstract void Write();
        }
    }
}