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
        public float Height
        {
            get => Controller.collider.height;
            set => Controller.collider.height = value;
        }
        public float Radius
        {
            get => Controller.collider.radius;
            set => Controller.collider.radius = value;
        }
        public float Multiplier { get; protected set; }

        public ControllerStateTransition Transition { get; protected set; }
        public ControllerStateSets Sets { get; protected set; }
        public ControllerStateElevationAdjustment HeightAdjustment { get; protected set; }

        public List<BaseControllerStateElement> Elements { get; protected set; }

        public class Module : FirstPersonController.BaseModule<ControllerState>
        {
            public ControllerState State => Reference;

            public override FirstPersonController Controller => State.Controller;
        }

        public Modules.Collection<ControllerState> Modules { get; protected set; }

        [Serializable]
        public struct Data : IData
        {
            [SerializeField]
            float height;
            public float Height { get { return height; } }

            [SerializeField]
            float radius;
            public float Radius { get { return radius; } }

            [SerializeField]
            float multiplier;
            public float Multiplier { get { return multiplier; } }

            public Data(float height, float radius, float multiplier)
            {
                this.height = height;

                this.radius = radius;

                this.multiplier = multiplier;
            }
            public Data(IData data) : this(data.Height, data.Radius, data.Multiplier) { }

            public static Data Zero => new Data(0f, 0f, 0f);

            public static Data Lerp(IData a, IData b, float rate)
            {
                return new Data()
                {
                    height = Mathf.Lerp(a.Height, b.Height, rate),
                    radius = Mathf.Lerp(a.Radius, b.Radius, rate),
                    multiplier = Mathf.Lerp(a.Multiplier, b.Multiplier, rate),
                };
            }

            public static Data operator *(Data a, float b)
            {
                return new Data()
                {
                    height = a.height * b,
                    radius = a.radius * b,
                    multiplier = a.multiplier * b,
                };
            }

            public static Data operator + (Data a, IData b)
            {
                return new Data()
                {
                    height = a.height + b.Height,
                    radius = a.radius + b.Radius,
                    multiplier = a.multiplier + b.Multiplier,
                };
            }
        }

        public interface IData
        {
            float Height { get; }

            float Radius { get; }

            float Multiplier { get; }
        }
        
        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<ControllerState>(this);

            Transition = Modules.Find<ControllerStateTransition>();
            Sets = Modules.Find<ControllerStateSets>();
            HeightAdjustment = Modules.Find<ControllerStateElevationAdjustment>();
            Elements = Modules.FindAll<BaseControllerStateElement>();

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Modules.Init();
        }

        public virtual void Set(IData data)
        {
            HeightAdjustment.Process(data);

            Height = data.Height;
            Radius = data.Radius;
            Multiplier = data.Multiplier;
        }
    }
}