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
    public class ControllerSprintAccelerationModifier : ControllerSprint.Module
    {
        [SerializeField]
        protected ValueRange scale = new ValueRange(1f, 2f);
        public ValueRange Scale { get { return scale; } }

        public float Value { get; protected set; }

        public float Modifier() => Value;

        public override void Configure()
        {
            base.Configure();

            Value = 1f;
        }

        public override void Initialize()
        {
            base.Initialize();

            Controller.Movement.Acceleration.Scale.Add(Modifier);

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if(Controller.IsGrounded)
                Value = scale.Lerp(Sprint.Weight);
        }
    }
}