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
	public class ControllerJumpPush : ControllerJump.Module
	{
        [SerializeField]
        protected ForceData force = new ForceData(100f, ForceMode.Impulse);
        public ForceData Force { get { return force; } }

        public ControllerGround Ground => Controller.Ground;

        public ControllerVelocity Velocity => Controller.Velocity;

        public override void Init()
        {
            base.Init();

            Jump.OnPerform += JumpCallback;
        }

        void JumpCallback()
        {
            if (Ground.IsDetected)
                Perform(Ground.Data);
        }

        protected virtual void Perform(ControllerGroundData hit)
        {
            if (hit.Rigidbody == null) return;

            var direction = CalculateDirection();

            hit.Rigidbody.AddForceAtPosition(direction * force.Value, hit.Point, force.Mode);
        }

        protected virtual Vector3 CalculateDirection()
        {
            var result = (Velocity.Right + Velocity.Forward).normalized;

            result += Jump.Direction;

            result = -result.normalized;

            return result;
        }
    }
}