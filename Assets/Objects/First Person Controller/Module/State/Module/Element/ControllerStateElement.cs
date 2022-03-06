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
    public abstract class ControllerStateElement : ControllerState.Module
    {
        [SerializeField]
        protected float transitionSpeed = 4;
        public float TransitionSpeed { get { return transitionSpeed; } }

        [SerializeField]
        protected ControllerStateData data = ControllerStateData.Default;
        public ControllerStateData Data { get { return data; } }

        public float Weight { get; protected set; }

        public bool Active => State.Transition.Target == this;

        public float Target => Active ? 1f : 0f;

        [field: SerializeField, DebugOnly]
        public Modules<ControllerStateElement> Modules { get; protected set; }
        public class Module : FirstPersonController.Behaviour, IModule<ControllerStateElement>
        {
            [field: SerializeField, DebugOnly]
            public ControllerStateElement Element { get; protected set; }

            public FirstPersonController Controller => Element.Controller;

            public virtual void Set(ControllerStateElement value) => Element = value;
        }

        public ControllerControls Controls => Controller.Controls;
        public ControllerStateTransition Transition => State.Transition;
        public ControllerStateSets Sets => State.Sets;

        public override void Set(ControllerState value)
        {
            base.Set(value);

            Modules = new Modules<ControllerStateElement>(this);
            Modules.Register(Controller.Behaviours);

            Modules.Set();
        }

        public override void Initialize()
        {
            base.Initialize();

            Weight = Target;

            Controller.OnProcess += Process;

            State.OnOperate += Operate;
        }

        protected virtual void Process()
        {
            Weight = Mathf.MoveTowards(Weight, Target, State.Transition.Speed * Time.deltaTime);
        }

        protected virtual void Operate()
        {

        }

        protected virtual void Toggle(ControllerStateElement normal)
        {
            if (Active)
                Transition.Set(normal);
            else
                Transition.Set(this);
        }
    }
}