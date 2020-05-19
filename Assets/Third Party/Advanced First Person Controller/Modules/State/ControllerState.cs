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

        public Data Current => new Data(Height, Radius, Multiplier);

        public ControllerStateTransition Transition { get; protected set; }

        public ControllerStateElevationAdjustment HeightAdjustment { get; protected set; }

        public class Module : FirstPersonController.Module
        {
            public ControllerState State => Controller.State;
        }

        public List<ControllerStateElement> Elements { get; protected set; }

        [Serializable]
        public struct Data
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

            public static Data Zero => new Data(0f, 0f, 0f);

            public static Data Lerp(Data a, Data b, float rate)
            {
                return new Data()
                {
                    height = Mathf.Lerp(a.height, b.height, rate),
                    radius = Mathf.Lerp(a.radius, b.radius, rate),
                    multiplier = Mathf.Lerp(a.multiplier, b.multiplier, rate),
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

            public static Data operator + (Data a, Data b)
            {
                return new Data()
                {
                    height = a.height + b.height,
                    radius = a.radius + b.radius,
                    multiplier = a.multiplier + b.multiplier,
                };
            }
        }
        
        public override void Configure(FirstPersonController reference)
        {
            base.Configure(reference);

            Transition = Dependancy.Get<ControllerStateTransition>(gameObject);

            HeightAdjustment = Dependancy.Get<ControllerStateElevationAdjustment>(gameObject);

            Elements = Dependancy.GetAll<ControllerStateElement>(Controller.gameObject);
        }

        public virtual void Set(Data data)
        {
            HeightAdjustment.Process(data);

            Height = data.Height;
            Radius = data.Radius;
            Multiplier = data.Multiplier;
        }
    }
}