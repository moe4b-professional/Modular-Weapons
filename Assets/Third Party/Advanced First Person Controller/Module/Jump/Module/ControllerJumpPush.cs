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
        protected float scale = 1f;
        public float Scale { get { return scale; } }

        public ControllerGround Ground => Controller.Ground;

        public ControllerVelocity Velocity => Controller.Velocity;

        public override void Init()
        {
            base.Init();

            Jump.OnDo += JumpCallback;
        }

        void JumpCallback()
        {
            if (Ground.IsDetected)
                Do(Ground.Data);
        }

        protected virtual void Do(ControllerGroundData hit)
        {
            if (hit.Rigidbody == null) return;

            var force = -Velocity.Absolute.normalized * Jump.Force.Value * scale;

            hit.Rigidbody.AddForceAtPosition(force, hit.Point, Jump.Force.Mode);
        }
    }
}