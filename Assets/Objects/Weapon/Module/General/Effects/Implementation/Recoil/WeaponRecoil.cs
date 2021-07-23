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
    public class WeaponRecoil : Weapon.Module, WeaponEffects.IInterface
    {
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        public Transform Context => anchor.transform;

        public Modifier.Scale Scale { get; protected set; }

        public class Module : Weapon.Behaviour, IModule<WeaponRecoil>
        {
            public WeaponRecoil Recoil { get; protected set; }
            public virtual void Set(WeaponRecoil value) => Recoil = value;

            public Weapon Weapon => Recoil.Weapon;
        }

        public Modules<WeaponRecoil> Modules { get; protected set; }

        public override void Set(Weapon value)
        {
            base.Set(value);

            Modules = new Modules<WeaponRecoil>(this);
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

            Weapon.Action.OnPerform += Action;
        }

        public event Action OnAction;
        void Action()
        {
            if (enabled) OnAction?.Invoke();
        }

        public abstract class Effect : Module
        {
            public ValueRange kick;

            [SerializeField]
            protected SwayData sway;
            public SwayData Sway { get { return sway; } }
            [Serializable]
            public struct SwayData
            {
                [SerializeField]
                float vertical;
                public float Vertical { get { return vertical; } }

                [SerializeField]
                float horizontal;
                public float Horizontal { get { return horizontal; } }

                public SwayData(float vertical, float horizontal)
                {
                    this.vertical = vertical;
                    this.horizontal = horizontal;
                }
                public SwayData(float value) : this(value, value)
                {

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

            [SerializeField]
            protected NoiseProperty noise;
            public NoiseProperty Noise { get { return noise; } }
            [Serializable]
            public class NoiseProperty
            {
                [SerializeField]
                [WeightValue("Random", "Perlin")]
                protected WeightValue weight = 0.6f;
                public WeightValue Weight
                {
                    get => weight;
                    set => weight = value;
                }

                public virtual float Sample(int seed)
                {
                    var random = Random.Range(0f, 1f) * weight.Left;
                    var perlin = Mathf.PerlinNoise(Time.time + seed + 10, Time.time + seed + 20) * weight.Right;

                    var eval = random + perlin;

                    return eval;
                }

                public virtual float Lerp(int seed, float min, float max)
                {
                    var sample = Sample(seed);

                    return Mathf.Lerp(min, max, sample);
                }
                public virtual float Lerp(int seed, ValueRange range) => Lerp(seed, range.Min, range.Max);
                public virtual float Lerp(int seed, float range) => Lerp(seed, -range, range);
            }

            public Transform Context => Recoil.Context;

            public Vector3 Target { get; protected set; }
            public Vector3 Value { get; protected set; }

            protected virtual void Reset()
            {

            }

            public override void Init()
            {
                base.Init();

                Recoil.OnAction += Action;

                Weapon.OnProcess += Process;

                Recoil.Anchor.OnWriteDefaults += Write;
            }

            void Action() => CalculateTarget();
            protected abstract void CalculateTarget();

            protected virtual void Process()
            {
                Value = Vector3.Lerp(Value, Target, speed.Set * Time.deltaTime);
                Target = Vector3.Lerp(Target, Vector3.zero, speed.Reset * Time.deltaTime);
            }

            protected abstract void Write();
        }
    }
}