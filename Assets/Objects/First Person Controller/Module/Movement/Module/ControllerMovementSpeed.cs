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
	public class ControllerMovementSpeed : ControllerMovement.Module
	{
		[SerializeField]
        protected float _base = 4f;
        public float Base { get { return _base; } }

        public float Current { get; protected set; }

        public float Max { get; protected set; }

        public ScaleModifer Scale { get; protected set; }
        public class ScaleModifer : Modifier.Scale { }

        public float Rate => Max == 0f ? 0f : Current / Max;

        public ControllerVelocity Velocity => Controller.Velocity;

        public override void Configure()
        {
            base.Configure();

            Calculate(1f);

            Scale = new ScaleModifer();
        }

        public virtual void Calculate(float multiplier)
        {
            Max = Evaluate(multiplier);

            Current = Velocity.Planar.magnitude;
        }

        public virtual float Evaluate(float multiplier) => Base * multiplier * Scale.Value;
    }
}