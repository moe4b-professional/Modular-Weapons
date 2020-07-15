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
        protected MultiplierData multiplier = new MultiplierData(1f, 1f);
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
        protected SpeedData speed;
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

        public WeaponPivot Pivot => Weapon.Pivot;
        public Transform Context => Pivot.transform;

        public class Module : Weapon.BaseModule<WeaponSway>
        {
            public WeaponSway Sway => Reference;

            public override Weapon Weapon => Reference.Weapon;
        }

        public Modules.Collection<WeaponSway> Modules { get; protected set; }

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            Transform Anchor { get; }

            Vector2 LookDelta { get; }

            Vector3 RelativeVelocity { get; }
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Scale = new Modifier.Scale();

            Modules = new Modules.Collection<WeaponSway>(this);

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

            Value = Vector2.Lerp(Value, Target, speed.Set * Time.deltaTime);

            Target = Vector2.Lerp(Value, Vector2.zero, speed.Reset * Time.deltaTime);

            OnProcess?.Invoke();
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
    }
}