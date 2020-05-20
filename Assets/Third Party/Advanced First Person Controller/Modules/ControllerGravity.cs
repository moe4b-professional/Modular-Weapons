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
	public class ControllerGravity : FirstPersonController.Module
	{
        [SerializeField]
        protected float value = 9.8f;
        public float Value { get { return value; } }

        [SerializeField]
        protected float multiplier = 1f;
        public float Multiplier { get { return multiplier; } }

        public Vector3 Direction { get; protected set; }

        public ControllerGroundCheck GroundCheck => Controller.GroundCheck;

        public override void Init()
        {
            base.Init();

            Controller.rigidbody.useGravity = false;
        }

        public virtual void Apply()
        {
            CalculateDirection();

            Controller.rigidbody.AddForce(Direction * value * multiplier, ForceMode.Acceleration);
        }

        protected virtual void CalculateDirection()
        {
            if (GroundCheck.IsGrounded)
                Direction = Vector3.Project(-Controller.transform.up, -GroundCheck.Hit.Normal);
            else
                Direction = -Controller.transform.up;
        }
    }
}