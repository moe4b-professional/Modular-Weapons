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
	public class WeaponAimLerpTransform : WeaponAim.Module
	{
		[SerializeField]
        protected Transform context;
        public Transform Context { get { return context; } }

        public Coordinates Idle { get; protected set; }

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("on")]
        protected Coordinates target;
        public Coordinates Target { get { return target; } }
        
        public Coordinates Offset { get; protected set; }

        protected virtual void Reset()
        {
            context = transform;

            Idle = new Coordinates(context);
            target = new Coordinates(context);
        }

        public override void Init()
        {
            base.Init();

            Weapon.OnLateProcess += LateProcess;

            Idle = new Coordinates(context);

            Offset = new Coordinates(Vector3.zero, Vector3.zero);
        }

        void LateProcess()
        {
            Apply(-Offset);

            Offset = Coordinates.Lerp(Coordinates.Zero, target - Idle, Aim.Rate);

            Apply(Offset);
        }

        protected virtual void Apply(Coordinates coordinates)
        {
            context.localPosition += coordinates.Position;
            context.localRotation *= coordinates.Rotation;
        }
    }
}