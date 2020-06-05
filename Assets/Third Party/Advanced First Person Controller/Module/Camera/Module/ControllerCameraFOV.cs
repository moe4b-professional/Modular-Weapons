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
	public class ControllerCameraFOV : ControllerCamera.Module
	{
		public float Initial { get; protected set; }

        public float Value
        {
            get => camera.Component.fieldOfView;
            set => camera.Component.fieldOfView = value;
        }

        public float Rate => Value / Initial;

        public ScaleModifier Scale { get; protected set; }
        public class ScaleModifier : Modifier.Scale<ControllerCameraFOV> { }

        public override void Configure()
        {
            base.Configure();

            Initial = Value;

            Scale = new ScaleModifier();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Value = Initial * Scale.Value;
        }
    }

    public class Modifier
    {
        public class Additive<TTarget> : Base<Additive<TTarget>.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 0f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Value;

                    return result;
                }
            }
        }

        public class Scale<TTarget> : Base<Scale<TTarget>.IInterface>
        {
            public interface IInterface
            {
                float Value { get; }
            }

            public float Value
            {
                get
                {
                    if (List.Count == 0) return 1f;

                    var result = 0f;

                    for (int i = 0; i < List.Count; i++)
                        result += List[i].Value;

                    return result - (List.Count - 1);
                }
            }
        }

        public class Base<TInterface>
            where TInterface : class
        {
            public List<TInterface> List { get; protected set; }

            public virtual void Register(TInterface modifier)
            {
                if (List.Contains(modifier))
                {
                    Debug.LogWarning("Modifier Already Added");
                    return;
                }

                List.Add(modifier);
            }

            public Base()
            {
                List = new List<TInterface>();
            }
        }
    }
}