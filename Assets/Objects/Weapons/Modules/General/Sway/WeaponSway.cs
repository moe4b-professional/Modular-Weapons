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
	public class WeaponSway : Weapon.Module
	{
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
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected EffectData effect = new EffectData(0.02f, 5f);
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

                public Vector3 Sample(Vector2 value)
                {
                    return new Vector3(value.x * horizontal, value.y * vertical);
                }

                public PositionData(float vertical, float horizontal)
                {
                    this.vertical = vertical;
                    this.horizontal = horizontal;
                }
                public PositionData(float value) : this(value, value)
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

                public Vector3 Sample(Vector2 value)
                {
                    return new Vector3(value.y * tilt, 0f, value.x * roll);
                }

                public RotationData(float roll, float tilt)
                {
                    this.roll = roll;
                    this.tilt = tilt;
                }
                public RotationData(float value) : this(value, value)
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

        public Vector2 Value { get; protected set; }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData)
                Process(data as IData);
        }

        void Process(IData data)
        {
            Value = Vector2.Lerp(Value, -data.Value, speed.Set * Time.deltaTime);

            Value = Vector2.ClampMagnitude(Value, 1f);

            Value = Vector2.Lerp(Value, Vector2.zero, speed.Reset * Time.deltaTime);
        }

        protected virtual void LateUpdate()
        {
            Weapon.transform.localPosition = effect.Position.Sample(Value) * scale;

            Weapon.transform.localEulerAngles = effect.Rotation.Sample(Value) * scale;
        }

        public interface IData
        {
            Vector2 Value { get; }
        }
    }
}