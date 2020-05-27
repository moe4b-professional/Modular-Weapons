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
	public class WeaponBob : Weapon.Module<WeaponBob.IProcessor>, Weapon.IEffect
    {
        [SerializeField]
        protected Transform context;
        public Transform Context { get { return context; } }

        [SerializeField]
        protected float scale = 1f;
        public float Scale
        {
            get => scale;
            set => scale = value;
        }

        [SerializeField]
        protected AnimationCurve curve;
        public AnimationCurve Curve { get { return curve; } }

        [SerializeField]
        protected float range = 0.02f;
        public float Range { get { return range; } }

        public Vector3 Offset { get; protected set; }

        public override void Init()
        {
            base.Init();

            Weapon.OnLateProcess += LateProcess;
        }

        void LateProcess()
        {
            if (HasProcessor) LateProcess(Processor);
        }
        protected virtual void LateProcess(IProcessor data)
        {
            context.localPosition -= Offset;

            Offset = Vector3.down * curve.Evaluate(data.Step) * range * scale;

            context.localPosition += Offset;
        }

        public interface IProcessor
        {
            float Step { get; }
        }
	}
}