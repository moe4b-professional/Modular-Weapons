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
        
        public override void Init()
        {
            base.Init();

            CalculateOffset();

            Controller.Anchors.OnLateProcess += LateProcess;
        }

        void LateProcess()
        {
            Angle = Mathf.Clamp(Angle - Look.Delta.y, -range, range);

            CalculateOffset();

            Rig.Camera.Anchor.LocalRotation *= Offset;
        }

        protected virtual void CalculateOffset()
        {
            Offset = Quaternion.AngleAxis(Angle, Axis);
        }
    }

    public static class QuatTool
    {
        public static void Add(Transform target, Quaternion rotation)
        {
            target.localRotation *= rotation;

            //target.localRotation = target.localRotation * rotation;
            //target.localRotation = rotation * target.localRotation;
        }

        public static void Subtract(Transform target, Quaternion rotation)
        {
            target.localRotation *= Quaternion.Inverse(rotation);

            //target.localRotation = Quaternion.Inverse(rotation) * target.localRotation;
            //target.localRotation = target.localRotation * Quaternion.Inverse(rotation);
        }
    }
}