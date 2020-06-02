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

        public float Rate { get; protected set; }

        public float Delta { get; protected set; }

        public int Count { get; protected set; }

        public ControllerVelocity Velocity => Controller.Velocity;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            var magnitde = Velocity.Planar.magnitude;

            if (magnitde < 0.01f || Controller.IsGrounded == false)
            {
                Delta = 0f;
            }
            else
            {
                Delta = magnitde * scale * Time.deltaTime;
                Rate += Delta;

                if (Rate >= 1f) Complete();
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