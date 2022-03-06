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
	public class ControllerMovementSpeed : ControllerMovement.Module
	{
		[SerializeField]
        protected float _base = 4f;
        public float Base { get { return _base; } }

        public float Current { get; protected set; }

        public float Max { get; protected set; }

        public Modifier.Scale Scale { get; protected set; }

        public float Rate => Max == 0f ? 0f : Current / Max;

        public ControllerVelocity Velocity => Controller.Velocity;
        public ControllerGround Ground => Controller.Ground;

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();
        }

        public virtual void Calculate()
        {
            Max = Base * Scale.Value;

            Read();
        }

        protected virtual void Read()
        {
            var planar = Velocity.Planar.magnitude;

            var vertical = Velocity.Up.magnitude;

            if (Controller.IsGrounded)
            {
                vertical *= planar / Max;
                vertical = Mathf.Clamp(vertical, 0f, Max - planar);

            }
            else
            {
                vertical = 0f;
            }

            Current = planar + vertical;
        }
    }
}