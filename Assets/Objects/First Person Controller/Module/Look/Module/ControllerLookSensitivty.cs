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
	public class ControllerLookSensitivty : ControllerLook.Module
	{
        [SerializeField]
        protected float _base = 5f;
        public float Base { get { return _base; } }

        public Modifier.Scale Scale { get; protected set; }

        public virtual float Value { get; protected set; }

        public override void Configure()
        {
            base.Configure();

            Scale = new Modifier.Scale();
        }

        public override void Init()
        {
            base.Init();

            Calculate();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Calculate();
        }

        protected virtual void Calculate()
        {
            Value = Base * Scale.Value;
        }
    }
}