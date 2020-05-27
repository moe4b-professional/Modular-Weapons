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
	public class StateEventSingleTrigger : AnimationTriggerRewind.StateMachine
	{
		[SerializeField]
        protected string _ID;
        public string ID { get { return _ID; } }

        [SerializeField]
        protected TriggerMode mode;
        public TriggerMode Mode { get { return mode; } }
        public enum TriggerMode
        {
            Enter, Exit
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            InitComponents(animator);

            if(Rewind != null)
            {
                if (mode == TriggerMode.Enter)
                    Rewind.Trigger(AnimationTrigger.Start.Format(ID));
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            InitComponents(animator);

            if (Rewind != null)
            {
                if (mode == TriggerMode.Exit)
                    Rewind.Trigger(AnimationTrigger.End.Format(ID));
            }
        }
    }
}