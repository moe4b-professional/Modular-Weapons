using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
    public class StateMachineEventsTrigger : AnimationTriggerRewind.StateMachine
    {
        [SerializeField]
        protected string _ID;
        public string ID { get { return _ID; } }
        public string FormatID(string prefix)
        {
            return ID + " " + prefix;
        }

        public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineEnter(animator, stateMachinePathHash);

            InitComponents(animator);

            if (Rewind != null) Rewind.Trigger(FormatID("Start"));
        }
        public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
        {
            base.OnStateMachineExit(animator, stateMachinePathHash);

            InitComponents(animator);

            if (Rewind != null) Rewind.Trigger(FormatID("End"));
        }
    }
}