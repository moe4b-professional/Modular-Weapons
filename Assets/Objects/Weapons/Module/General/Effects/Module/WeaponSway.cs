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
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

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
        protected SpeedData speed = new SpeedData(2f, 3f);
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
        protected EffectData effect = new EffectData(0.01f, 5f);
        public EffectData Effect { get { return effect; } }
        [Serializable]
        public struct EffectData
        {
            [SerializeField]
            PositionData position;
            public PositionData Position { get { return position; } }
            [Serializable]
            public struct PositionData
            {
                [SerializeField]
                float vertical;
                public float Vertical { get { return vertical; } }

                [SerializeField]
                float horizontal;
                public float Horizontal { get { return horizontal; } }

                [SerializeField]
                float fordical;
                public float Fordical { get { return fordical; } }

                public Vector3 Sample(Vector3 value)
                {
                    return new Vector3(value.x * horizontal, value.y * vertical, value.z * fordical);
                }

                public PositionData(float vertical, float horizontal, float fordical)
                {
                    this.vertical = vertical;
                    this.horizontal = horizontal;
                    this.fordical = fordical;
                }
                public PositionData(float value) : this(value, value * 2, value * 3)
                {

                }
            }

            [SerializeField]
            RotationData rotation;
            public RotationData Rotation { get { return rotation; } }
            [Serializable]
            public struct RotationData
            {
                [SerializeField]
                float roll;
                public float Roll { get { return roll; } }

                [SerializeField]
                float tilt;
                public float Tilt { get { return tilt; } }

                public Vector3 Sample(Vector3 value)
                {
                    return new Vector3(value.y * tilt, 0f, value.x * roll);
                }

                public RotationData(float roll, float tilt)
                {
                    this.roll = roll;
                    this.tilt = tilt;
                }
                public RotationData(float value) : this(value, value / 2f)
                {

                }
            }

            public EffectData(PositionData position, RotationData rotation)
            {
                this.position = position;
                this.rotation = rotation;
            }
            public EffectData(float position, float rotation) : this(new PositionData(position), new RotationData(rotation))
            {

            }
        }

        public Vector3 Target { get; protected set; }
        public Vector3 Value { get; protected set; }

        public Vector3 Position { get; protected set; } = Vector3.zero;
        public Vector3 Rotation { get; protected set; } = Vector3.zero;

        public Transform Context => Pivot.transform;

        public WeaponPivot Pivot => Weapon.Pivot;

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            Vector3 LookDelta { get; }

            Vector3 RelativeVelocity { get; }
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();
        }

        public override void Init()
        {
            base.Init();

            Pivot.OnProcess += Process;
        }

        void Process()
        {
            CalculateTarget();

            Value = Vector3.Lerp(Value, Target, speed.Set * Time.deltaTime);
            Target = Vector3.Lerp(Value, Vector3.zero, speed.Reset * Time.deltaTime);

            Position = effect.Position.Sample(Value) * scale;
            Rotation = effect.Rotation.Sample(Value) * scale;

            Context.localPosition += Position;
            Context.localEulerAngles += Rotation;
        }

        protected virtual void CalculateTarget()
        {
            Target = Vector3.zero;

            if(enabled)
            {
                Target -= Processor.LookDelta * multiplier.Look;

                Target += Vector3.left * Processor.RelativeVelocity.x * multiplier.Move;

                Target += Vector3.back * Mathf.Abs(Processor.RelativeVelocity.z) * multiplier.Move;
            }
        }
    }
}