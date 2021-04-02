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
	public class WeaponProjectileForce : WeaponProjectileAction.Module
	{
        [SerializeField]
        protected float value = 5f;
        public float Value { get { return value; } }

        [SerializeField]
        protected Vector3 axis = Vector3.forward;
        public Vector3 Axis { get { return axis; } }

        [SerializeField]
        protected ForceMode mode = ForceMode.VelocityChange;
        public ForceMode Mode { get { return mode; } }

        public override void Init()
        {
            base.Init();

            Action.OnPerform += ActionCallback;
        }

        void ActionCallback(Projectile projectile)
        {
            if (enabled == false) return;

            var direction = Action.Point.TransformDirection(axis).normalized;

            Debug.DrawRay(Action.Point.position, direction * 20, Color.red, 4f);

            projectile.Motor.Velocity = value * direction;
        }
    }
}