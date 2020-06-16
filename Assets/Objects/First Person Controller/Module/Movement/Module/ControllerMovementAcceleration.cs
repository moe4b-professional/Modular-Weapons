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
	public class ControllerMovementAcceleration : ControllerMovement.Module
	{
        [SerializeField]
        protected float _base = 15f;
        public float Base { get { return _base; } }

        public float Value { get; protected set; }

        public Modifier.Scale Scale { get; protected set; }

        public ControllerVelocity Velocity => Controller.Velocity;

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();
        }

        public virtual void Calculate()
        {
            Value = Base * Scale.Value;
        }
    }
}