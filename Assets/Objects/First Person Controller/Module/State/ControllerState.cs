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
    public class ControllerState : FirstPersonController.Module
    {
        public ControllerStateData Data { get; protected set; }

        public ControllerStateTransition Transition { get; protected set; }
        public ControllerStateSets Sets { get; protected set; }
        public ControllerStateElevationAdjustment HeightAdjustment { get; protected set; }

        public List<ControllerStateElement> Elements { get; protected set; }

        public class Module : FirstPersonController.BaseModule<ControllerState>
        {
            public ControllerState State => Reference;

            public override FirstPersonController Controller => State.Controller;
        }

        public Modules.Collection<ControllerState> Modules { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Data = ControllerStateData.Read(Controller);

            Modules = new Modules.Collection<ControllerState>(this);

            Transition = Modules.Depend<ControllerStateTransition>();
            Sets = Modules.Depend<ControllerStateSets>();
            HeightAdjustment = Modules.Depend<ControllerStateElevationAdjustment>();
            Elements = Modules.FindAll<ControllerStateElement>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }

        public event Action OnOperate;
        public virtual void Operate()
        {
            OnOperate?.Invoke();
        }

        public virtual void Set(ControllerStateData target)
        {
            HeightAdjustment.Process(target);

            Data = target;

            UpdateState();
        }

        protected virtual void UpdateState()
        {
            Controller.Height = Data.Height;

            Controller.Radius = Data.Radius;
        }
    }

    [Serializable]
    public struct ControllerStateData
    {
        [SerializeField]
        float height;
        public float Height { get { return height; } }

        [SerializeField]
        float radius;
        public float Radius { get { return radius; } }

        [SerializeField]
        MultiplierData multiplier;
        public MultiplierData Multiplier { get { return multiplier; } }
        [Serializable]
        public struct MultiplierData
        {
            [SerializeField]
            float speed;
            public float Speed { get { return speed; } }

            [SerializeField]
            float acceleration;
            public float Acceleration { get { return acceleration; } }

            public static MultiplierData Lerp(MultiplierData a, MultiplierData b, float rate)
            {
                return new MultiplierData()
                {
                    speed = Mathf.Lerp(a.speed, b.speed, rate),
                    acceleration = Mathf.Lerp(a.acceleration, b.acceleration, rate),
                };
            }

            public static MultiplierData operator *(MultiplierData a, float b)
            {
                return new MultiplierData()
                {
                    speed = a.speed * b,
                    acceleration = a.acceleration * b,
                };
            }

            public static MultiplierData operator +(MultiplierData a, MultiplierData b)
            {
                return new MultiplierData()
                {
                    speed = a.speed + b.speed,
                    acceleration = a.acceleration + b.acceleration,
                };
            }

            public MultiplierData(float speed, float acceleration)
            {
                this.speed = speed;
                this.acceleration = acceleration;
            }
            public MultiplierData(float value) : this(value, value) 
            {

            }

            public static MultiplierData Zero => new MultiplierData(0f, 0f);
        }

        public ControllerStateData(float height, float radius, MultiplierData multiplier)
        {
            this.height = height;

            this.radius = radius;

            this.multiplier = multiplier;
        }

        //Static Utility

        public static ControllerStateData Zero => new ControllerStateData(0f, 0f, MultiplierData.Zero);

        public static ControllerStateData Default
        {
            get
            {
                return new ControllerStateData()
                {
                    height = 1.8f,
                    radius = 0.4f,
                    multiplier = new MultiplierData(1)
                };
            }
        }

        public static ControllerStateData Read(FirstPersonController controller)
        {
            var data = new ControllerStateData()
            {
                height = controller.Height,
                radius = controller.Radius,
            };

            return data;
        }

        public static ControllerStateData Lerp(ControllerStateData a, ControllerStateData b, float rate)
        {
            return new ControllerStateData()
            {
                height = Mathf.Lerp(a.Height, b.Height, rate),
                radius = Mathf.Lerp(a.Radius, b.Radius, rate),
                multiplier = MultiplierData.Lerp(a.Multiplier, b.Multiplier, rate),
            };
        }

        public static ControllerStateData operator *(ControllerStateData a, float b)
        {
            return new ControllerStateData()
            {
                height = a.height * b,
                radius = a.radius * b,
                multiplier = a.multiplier * b,
            };
        }

        public static ControllerStateData operator +(ControllerStateData a, ControllerStateData b)
        {
            return new ControllerStateData()
            {
                height = a.height + b.Height,
                radius = a.radius + b.Radius,
                multiplier = a.multiplier + b.Multiplier,
            };
        }
    }

}