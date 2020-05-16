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
    public class ControllerStateElement : ControllerState.Module
    {
        public float Weight { get; protected set; }

        public bool Active => State.Transition.Target == this;

        public float Target => Active ? 1f : 0f;

        [SerializeField]
        protected float transitionSpeed;
        public float TransitionSpeed { get { return transitionSpeed; } }

        [SerializeField]
        protected ControllerState.Data data;
        public ControllerState.Data Data { get { return data; } }

        public override void Init()
        {
            base.Init();

            Weight = Target;

            Controller.OnProcess += Process;
        }

        void Process()
        {
            Weight = Mathf.MoveTowards(Weight, Target, State.Transition.Speed * Time.deltaTime);
        }
    }
}