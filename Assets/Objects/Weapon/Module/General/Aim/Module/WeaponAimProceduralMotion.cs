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
	public class WeaponAimProceduralMotion : WeaponAim.Module
	{
        [SerializeField]
        protected TransformAnchor anchor;
        public TransformAnchor Anchor { get { return anchor; } }

        public Transform Context => anchor.transform;

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

        public Vector3 Position { get; protected set; }
        public Vector3 Rotation { get; protected set; }

        public override void Initialize()
        {
            base.Initialize();

            Weapon.OnProcess += Process;

            anchor.OnWriteDefaults += Write;
        }

        void Process()
        {
            Position = EvaluatePosition(Aim.Rate);
            Rotation = EvaluateRotation(Aim.Rate);
        }
        public virtual Vector3 EvaluatePosition(float rate)
        {
            var value = new Vector3()
            {
                x = curve.Evaluate(rate) * scale.Position.x,
                y = curve.Evaluate(rate) * scale.Position.y,
                z = curve.Evaluate(rate) * scale.Position.z,
            };

            return value;
        }
        protected virtual Vector3 EvaluateRotation(float rate)
        {
            var value = new Vector3()
            {
                x = curve.Evaluate(rate) * scale.Rotation.x,
                y = curve.Evaluate(rate) * scale.Rotation.y,
                z = curve.Evaluate(rate) * scale.Rotation.z,
            };

            return value;
        }

        protected virtual void Write()
        {
            Context.localPosition += Position;
            Context.localEulerAngles += Rotation;
        }
    }
}