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
	public class ControllerStateElementSprint : ControllerStateElement.Module, Modifier.Constraint.IInterface
    {
        [SerializeField]
        protected InputAction action = InputAction.StandUp;
        public InputAction Action { get { return action; } }
        public enum InputAction
        {
            Sprint, StandUp
        }

        bool Modifier.Constraint.IInterface.Active => action == InputAction.StandUp && Element.Weight > 0f;

        public ControllerSprint Sprint => Controller.Sprint;

        public ControllerState State => Controller.State;

        public override void Init()
        {
            base.Init();

            Sprint.Constraint.Register(this);

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if(Element.Active && action == InputAction.StandUp)
            {
                if(Sprint.Input.Button.Press)
                    State.Transition.Set(State.Sets.Normal);
            }
        }
    }
}