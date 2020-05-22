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
	public class ControllerCameraLook : ControllerLook.Module
	{
        [SerializeField]
        [Range(0f, MaxRange)]
        protected float range = 80f;
        public float Range
        {
            get => range;
            set => range = Mathf.Clamp(value, 0, MaxRange);
        }

        public const float MaxRange = 90f;

        public Vector3 Axis => Vector3.right;

        public float Angle { get; protected set; }

        public Quaternion Offset { get; protected set; }

        public Transform Context => Controller.Rig.camera.Component.transform;

        public override void Init()
        {
            base.Init();

            CalculateOffset();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Context.localRotation *= Quaternion.Inverse(Offset);

            Angle = Mathf.Clamp(Angle - Look.Delta.y, -range, range);

            CalculateOffset();

            Context.localRotation *= Offset;
        }

        protected virtual void CalculateOffset()
        {
            Offset = Quaternion.Euler(Angle, 0f, 0f);
        }
    }
}