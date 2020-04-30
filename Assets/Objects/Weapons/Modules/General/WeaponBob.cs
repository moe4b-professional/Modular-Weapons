﻿using System;
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
	public class WeaponBob : Weapon.Module
	{
        [SerializeField]
        protected Transform context;
        public Transform Context { get { return context; } }

        [SerializeField]
        protected AnimationCurve curve;
        public AnimationCurve Curve { get { return curve; } }

        [SerializeField]
        protected float range;
        public float Range { get { return range; } }

        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected float stepLength = 1.5f;
        public float StepLength { get { return stepLength; } }

        public float Distance { get; protected set; }

        public float Rate => Distance / stepLength;

        [SerializeField]
        protected float resetSpeed;
        public float ResetSpeed { get { return resetSpeed; } }

        public Vector3 Offset { get; protected set; }

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Process;
        }

        void Process(Weapon.IProcessData data)
        {
            if (data is IData)
                Process(data as IData);
        }
        protected virtual void Process(IData data)
        {
            var velocity = Vector3.Scale(data.Velocity, Vector3.right + Vector3.forward);
            var magnitude = velocity.magnitude;

            if (magnitude > 0f)
            {
                Distance += magnitude * Time.deltaTime;
            }
            else
            {
                var target = Distance >= stepLength / 2f ? stepLength : 0f;

                Distance = Mathf.MoveTowards(Distance, target, resetSpeed * Time.deltaTime);
            }

            while (Distance >= stepLength) Distance -= stepLength;
        }

        protected virtual void LateUpdate()
        {
            context.localPosition -= Offset;
            {
                Offset = Vector3.up * curve.Evaluate(Rate) * range * scale;
            }
            context.localPosition += Offset;
        }

        public interface IData
        {
            Vector3 Velocity { get; }
        }
	}
}