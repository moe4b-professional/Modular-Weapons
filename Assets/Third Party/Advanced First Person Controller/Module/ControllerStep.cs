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
	public class ControllerStep : FirstPersonController.Module
	{
        [SerializeField]
        protected float scale;
        public float Scale { get { return scale; } }

        [SerializeField]
        protected WeightData weight;
        public WeightData Weight { get { return weight; } }
        [Serializable]
        public class WeightData
        {
            [SerializeField]
            protected float acceleration = 5f;
            public float Acceleration { get { return acceleration; } }

            public float Value { get; protected set; }

            public virtual void Process(float target)
            {
                Value = Mathf.MoveTowards(Value, target, acceleration * Time.deltaTime);
            }
        }

        public float Rate { get; protected set; }

        public float Delta { get; protected set; }

        public int Count { get; protected set; }

        public ControllerVelocity Velocity => Controller.Velocity;

        public ControllerMovement Movement => Controller.Movement;

        public ControllerMovementSpeed Speed => Movement.Speed;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if (Speed.Current > 0.01f && Controller.IsGrounded)
            {
                Delta = Speed.Current * scale * Time.deltaTime;

                Rate += Delta;

                Weight.Process(1f);

                if (Rate >= 1f) Complete();
            }
            else
            {
                Delta = 0f;

                Weight.Process(0f);

                if (weight.Value == 0f) Rate = 0f;
            }
        }

        public event Action OnComplete;
        protected virtual void Complete()
        {
            while(Rate > 1f)
            {
                Count++;
                Rate -= 1f;
            }

            OnComplete?.Invoke();
        }
    }
}