using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
    public class StateEventMultiTrigger : AnimationTriggerRewind.StateMachine
    {
        [SerializeField]
        protected string _ID;
        public string ID { get { return _ID; } }

        [SerializeField]
        protected AnimationCurveToggleValue curve;
        public AnimationCurveToggleValue Curve { get { return curve; } }
        protected virtual void SetCurve(float time)
        {
            Rewind.Curves.Set(ID, curve.Evaluate(time));
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            InitComponents(animator);

            if (Rewind != null)
            {
                Rewind.Trigger(AnimationTrigger.Start.Format(ID));

                SetCurve(stateInfo.normalizedTime);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            InitComponents(animator);

            if (Rewind != null)
            {
                SetCurve(stateInfo.normalizedTime);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            InitComponents(animator);

            if (Rewind != null)
            {
                Rewind.Trigger(AnimationTrigger.End.Format(ID));

                SetCurve(0f);
            }
        }
    }
}