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
	public class WeaponBob : Weapon.Module, WeaponEffects.IInterface
    {
        public Modifier.Scale Scale { get; protected set; }

        [SerializeField]
        protected float range = 0.0035f;
        public float Range { get { return range; } }

        public Transform Context => Pivot.transform;

        public Vector3 Offset { get; protected set; }

        public WeaponPivot Pivot => Weapon.Pivot;

        public IProcessor Processor { get; protected set; }
        public interface IProcessor
        {
            Vector3 Delta { get; }
        }

        public override void Configure()
        {
            base.Configure();

            Processor = GetProcessor<IProcessor>();

            Scale = new Modifier.Scale();

            Weapon.Effects.Register(this);
        }

        public override void Init()
        {
            base.Init();

            Pivot.OnProcess += Process;
        }

        void Process()
        {
            Offset = Processor.Delta * range * Scale.Value;

            Context.localPosition += Offset;
        }
	}
}