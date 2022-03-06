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
	public class ControllerStateTransition : ControllerState.Module
    {
        public float Speed { get; protected set; }

        [SerializeField]
        protected ControllerStateElement target;
        public ControllerStateElement Target { get { return target; } }

        public IReadOnlyList<ControllerStateElement> Elements => State.Elements;

        public override void Configure()
        {
            base.Configure();

            if (target == null && Elements.Count > 0)
                target = Elements[0];
        }
        public override void Initialize()
        {
            base.Initialize();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            CalculateSpeed();

            ProcessElements();
        }

        protected virtual void CalculateSpeed()
        {
            Speed = 0f;

            for (int i = 0; i < Elements.Count; i++)
                Speed += Elements[i].TransitionSpeed * Elements[i].Weight;
        }

        protected virtual void ProcessElements()
        {
            var data = ControllerStateData.Zero;

            for (int i = 0; i < Elements.Count; i++)
                data += Elements[i].Data * Elements[i].Weight;

            State.Set(data);
        }

        public delegate void SetDelegate(ControllerStateElement target);
        public event SetDelegate OnSet;
        public virtual void Set(ControllerStateElement value)
        {
            target = value;

            OnSet?.Invoke(target);
        }
    }
}