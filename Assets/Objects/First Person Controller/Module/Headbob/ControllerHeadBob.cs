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

using MB;

namespace Game
{
	public class ControllerHeadBob : FirstPersonController.Module
	{
        [SerializeField]
        protected float range = 0.05f;
        public float Range { get { return range; } }

        [SerializeField]
        protected ScaleData scale;
        public ScaleData Scale { get { return scale; } }
        [Serializable]
        public class ScaleData
        {
            [SerializeField]
            protected float vertical = 0.5f;
            public float Vertical { get { return vertical; } }

            [SerializeField]
            protected float horizontal = 1f;
            public float Horizontal { get { return horizontal; } }
        }

        [SerializeField]
        protected float speed = 4f;
        public float Speed { get { return speed; } }

        public class Module : FirstPersonController.Behaviour, IModule<ControllerHeadBob>
        {
            public ControllerHeadBob HeadBob { get; protected set; }
            public virtual void Set(ControllerHeadBob value) => HeadBob = value;

            public FirstPersonController Controller => HeadBob.Controller;
        }
        public Modules<ControllerHeadBob> Modules { get; protected set; }

        public Vector3 Delta { get; protected set; }
        public Vector3 Offset { get; protected set; }

        public ControllerStep Step => Controller.Step;

        public override void Set(FirstPersonController value)
        {
            base.Set(value);

            Modules = new Modules<ControllerHeadBob>(this);
            Modules.Register(Controller.Behaviours);

            Modules.Set();
        }

        public override void Configure()
        {
            base.Configure();

            Delta = Offset = Vector3.zero;
        }

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Delta = new Vector3()
            {
                x = scale.Horizontal * Mathf.Sin(speed * Step.Time),
                y = scale.Vertical * Mathf.Sin(speed * 2 * Step.Time)
            };

            Delta *= Step.Weight.Value;

            Offset = Delta * range;
        }
    }
}