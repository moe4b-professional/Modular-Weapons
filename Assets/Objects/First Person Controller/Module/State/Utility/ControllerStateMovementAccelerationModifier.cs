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
	public class ControllerStateMovementAccelerationModifier : ControllerState.Module, Modifier.Scale.IInterface
	{
        public float Value { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Value = 1f;
        }

        public override void Init()
        {
            base.Init();

            Controller.Movement.Acceleration.Scale.Register(this);

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if (Controller.IsGrounded)
                Value = State.Data.Multiplier;
        }
    }
}