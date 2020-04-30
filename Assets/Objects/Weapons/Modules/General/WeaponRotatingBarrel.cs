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
	public class WeaponRotatingBarrel : Weapon.Module, Weapon.IConstraint, WeaponRPM.IModifier
	{
		[SerializeField]
        protected Transform context;
        public Transform Context { get { return context; } }

        [SerializeField]
        protected Vector3 axis = Vector3.forward;
        public Vector3 Axis { get { return axis; } }

        [SerializeField]
        protected AccelerationData acceleration;
        public AccelerationData Acceleration { get { return acceleration; } }
        [Serializable]
        public struct AccelerationData
        {
            [SerializeField]
            float set;
            public float Set { get { return set; } }

            [SerializeField]
            float reset;
            public float Reset { get { return reset; } }

            public AccelerationData(float set, float reset)
            {
                this.set = set;
                this.reset = reset;
            }
        }

        [SerializeField]
        protected float rotation = 20f;
        public float Rotation { get { return rotation; } }

        public float Rate { get; protected set; }

        [SerializeField]
        [Range(0f, 1f)]
        protected float activationRate = 0.6f;
        public float ActivationRate { get { return activationRate; } }

        float WeaponRPM.IModifier.Multiplier => Rate;

        bool Weapon.IConstraint.Active => Rate < activationRate;

        public override void Init()
        {
            base.Init();

            Weapon.OnProcess += Porcess;
        }

        void Porcess(Weapon.IProcessData data)
        {
            if (data.Input)
                Rate = Mathf.MoveTowards(Rate, 1f, acceleration.Set * Time.deltaTime);
            else
                Rate = Mathf.MoveTowards(Rate, 0f, acceleration.Reset * Time.deltaTime);

            context.Rotate(axis * rotation * Rate, Space.Self);
        }
    }
}