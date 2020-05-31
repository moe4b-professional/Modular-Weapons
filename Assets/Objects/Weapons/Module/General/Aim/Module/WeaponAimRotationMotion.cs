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
	public class WeaponAimRotationMotion : WeaponAim.Module
	{
        [SerializeField]
        protected AnimationCurve curve;
        public AnimationCurve Curve { get { return curve; } }

        [SerializeField]
        protected ScaleData scale;
        public ScaleData Scale { get { return scale; } }
        [Serializable]
        public class ScaleData
        {
            [SerializeField]
            protected Vector3 position = new Vector3(-0.0025f, -0.0025f, -0.02f);
            public Vector3 Position { get { return position; } }

            [SerializeField]
            protected Vector3 rotation = new Vector3(5, 2, 10);
            public Vector3 Rotation { get { return rotation; } }
        }

        [Serializable]
        public class AxisData
        {
            [SerializeField]
            protected float scale;
            public float Scale { get { return scale; } }

            [SerializeField]
            protected AnimationCurve curve;
            public AnimationCurve Curve { get { return curve; } }

            public float Evaluate(float rate) => curve.Evaluate(rate) * scale;
        }

        public WeaponPivot Pivot => Weapon.Pivot;

        public Transform Context => Pivot.transform;

        public Quaternion Offset { get; protected set; }

        public override void Init()
        {
            base.Init();

            Pivot.OnProcess += Process;
        }

        void Process()
        {
            Evaluate(Aim.Rate, out var position, out var angles);

            Context.localPosition += position;
            Context.localRotation *= Quaternion.Euler(angles);
        }

        protected virtual void Evaluate(float rate, out Vector3 position, out Vector3 angles)
        {
            position = new Vector3()
            {
                x = curve.Evaluate(rate) * scale.Position.x,
                y = curve.Evaluate(rate) * scale.Position.y,
                z = curve.Evaluate(rate) * scale.Position.z,
            };

            angles = new Vector3()
            {
                x = curve.Evaluate(rate) * scale.Rotation.x,
                y = curve.Evaluate(rate) * scale.Rotation.y,
                z = curve.Evaluate(rate) * scale.Rotation.z,
            };
        }
    }
}