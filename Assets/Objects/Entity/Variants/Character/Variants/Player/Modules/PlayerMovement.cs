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
	public class PlayerMovement : Player.Module
	{
        [SerializeField]
        protected float speed = 5f;
        public float Speed { get { return speed; } }

        [SerializeField]
        protected float acceleration = 15f;
        public float Acceleration { get { return acceleration; } }

        public override void Init()
        {
            base.Init();

            Player.OnProcess += Process;
        }

        void Process()
        {
            var input = new Vector2()
            {
                x = Input.GetAxisRaw("Horizontal"),
                y = Input.GetAxisRaw("Vertical")
            };

            var target = Player.transform.forward * input.y + Player.transform.right * input.x;
            target *= speed;
            target = Vector3.ClampMagnitude(target, speed);

            var velocity = Player.rigidbody.velocity;
            velocity.y = 0f;

            velocity = Vector3.MoveTowards(velocity, target, acceleration * Time.deltaTime);

            velocity.y = Player.rigidbody.velocity.y;

            Player.rigidbody.velocity = velocity;
        }
    }
}