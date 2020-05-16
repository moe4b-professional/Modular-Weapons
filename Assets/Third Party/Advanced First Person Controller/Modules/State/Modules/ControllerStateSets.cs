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
	public class ControllerStateSets : ControllerState.Module
	{
        [SerializeField]
        protected ControllerStateElement normal;
        public ControllerStateElement Normal { get { return normal; } }

        [SerializeField]
        protected ControllerStateElement crouch;
        public ControllerStateElement Crouch { get { return crouch; } }

        [SerializeField]
        protected ControllerStateElement prone;
        public ControllerStateElement Prone { get { return prone; } }

        public ControllerStateTransition Transition => State.Transition;

        public override void Init()
        {
            base.Init();

            Controller.OnProcess += Process;
        }

        void Process()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (crouch.Active)
                    Transition.Set(normal);
                else
                    Transition.Set(crouch);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (prone.Active)
                    Transition.Set(normal);
                else
                    Transition.Set(prone);
            }

            if (Input.GetKeyDown(KeyCode.Space))
                State.Transition.Set(normal);
        }
    }
}