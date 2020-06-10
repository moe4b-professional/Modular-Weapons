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
        protected ControllerNormalStateElement normal;
        public ControllerNormalStateElement Normal { get { return normal; } }

        [SerializeField]
        protected ControllerCrouchStateElement crouch;
        public ControllerCrouchStateElement Crouch { get { return crouch; } }

        [SerializeField]
        protected ControllerProneStateElement prone;
        public ControllerProneStateElement Prone { get { return prone; } }

        public ControllerStateTransition Transition => State.Transition;

        public ControllerInput Input => Controller.Input;
    }
}