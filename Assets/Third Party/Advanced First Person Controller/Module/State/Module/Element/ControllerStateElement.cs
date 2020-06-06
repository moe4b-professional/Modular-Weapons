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
    public abstract class ControllerStateElement : ControllerState.Module, ControllerState.IData
    {
        public float Weight { get; protected set; }

        public bool Active => State.Transition.Target == this;

        public float Target => Active ? 1f : 0f;

        public abstract float Height { get; }
        public abstract float Radius { get; }
        public abstract float Multiplier { get; }

        [SerializeField]
        protected float transitionSpeed = 4;
        public float TransitionSpeed { get { return transitionSpeed; } }

        public class Module : FirstPersonController.BaseModule<ControllerStateElement>
        {
            public ControllerStateElement Element => Reference;

            public override FirstPersonController Controller => Reference.Controller;
        }

        public Modules.Collection<ControllerStateElement> Modules { get; protected set; }

        public ControllerInput Input => Controller.Input;
        public ControllerStateTransition Transition => State.Transition;
        public ControllerStateSets Sets => State.Sets;

        public override void Configure()
        {
            base.Configure();

            Modules = new Modules.Collection<ControllerStateElement>(this);

            Modules.Configure();
        }

        public override void Init()
        {
            base.Init();

            Weight = Target;

            Controller.OnProcess += Process;

            Modules.Init();
        }

        protected virtual void Process()
        {
            Weight = Mathf.MoveTowards(Weight, Target, State.Transition.Speed * Time.deltaTime);
        }

        protected virtual void Toggle(ControllerStateElement normal)
        {
            if (Active)
                Transition.Set(normal);
            else
                Transition.Set(this);
        }
    }

    public class DefaultControllerStateElement : ControllerStateElement
    {
        [SerializeField]
        protected float height;
        public override float Height => height;

        [SerializeField]
        protected float radius;
        public override float Radius => radius;

        [SerializeField]
        protected float multiplier;
        public override float Multiplier => multiplier;
    }
}