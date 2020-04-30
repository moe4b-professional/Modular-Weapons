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
    public class WeaponRecoil : Weapon.Module
    {
        [SerializeField]
        protected float scale;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected PositionProcessor position;
        public PositionProcessor Position { get { return position; } }
        [Serializable]
        public class PositionProcessor : DimensionProcessor
        {
            protected override void Apply(Vector3 value) => context.localPosition += value;

            protected override Vector3 CalculateTarget()
            {
                return new Vector3()
                {
                    x = Random.Range(-sway.Horizontal, sway.Horizontal),
                    y = Random.Range(-sway.Vertical, sway.Vertical),
                    z = kick.Random
                };
            }
        }

        [SerializeField]
        protected RotationProcessor rotation;
        public RotationProcessor Rotation { get { return rotation; } }
        [Serializable]
        public class RotationProcessor : DimensionProcessor
        {
            protected override void Apply(Vector3 value) => context.localEulerAngles += value;

            protected override Vector3 CalculateTarget()
            {
                return new Vector3()
                {
                    x = kick.Random,
                    y = Random.Range(-sway.Vertical, sway.Vertical),
                    z = Random.Range(-sway.Vertical, sway.Vertical)
                };
            }
        }

        [Serializable]
        public abstract class DimensionProcessor
        {
            [SerializeField]
            protected Transform context;
            public Transform Context
            {
                get => context;
                set => context = value;
            }

            public ValueRange kick;

            [SerializeField]
            protected SwayData sway;
            public SwayData Sway { get { return sway; } }

            [SerializeField]
            protected SpeedData speed;
            public SpeedData Speed { get { return speed; } }

            public Vector3 Target { get; protected set; }

            public Vector3 Value { get; protected set; }

            public virtual void Action(float scale)
            {
                Target = CalculateTarget() * scale;
            }

            protected abstract Vector3 CalculateTarget();

            public virtual void Apply()
            {
                Apply(-Value);
                {
                    Value = Vector3.Lerp(Value, Target, speed.Set * Time.deltaTime);
                    Target = Vector3.Lerp(Target, Vector3.zero, speed.Reset * Time.deltaTime);
                }
                Apply(Value);
            }

            protected abstract void Apply(Vector3 value);
        }

        [Serializable]
        public struct ValueRange
        {
            [SerializeField]
            float min;
            public float Min { get { return min; } }

            [SerializeField]
            float max;
            public float Max { get { return max; } }

            public float Random => UnityEngine.Random.Range(min, max);

            public float Lerp(float t) => Mathf.Lerp(min, max, t);

            public ValueRange(float min, float max)
            {
                this.min = min;
                this.max = max;
            }
        }

        [Serializable]
        public class SwayData
        {
            [SerializeField]
            protected float vertical;
            public float Vertical { get { return vertical; } }

            [SerializeField]
            protected float horizontal;
            public float Horizontal { get { return horizontal; } }
        }

        [Serializable]
        public class SpeedData
        {
            [SerializeField]
            protected float set;
            public float Set { get { return set; } }

            [SerializeField]
            protected float reset;
            public float Reset { get { return reset; } }
        }
        
        protected virtual void Reset()
        {
            position.Context = transform;
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnAction += Action;
        }

        void Action()
        {
            position.Action(scale);
            rotation.Action(scale);
        }

        protected virtual void LateUpdate()
        {
            position.Apply();
            rotation.Apply();
        }
    }
}