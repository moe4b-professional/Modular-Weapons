using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Game
{
    public class StateMachineEventTrigger : StateMachineBehaviour
    {
        [SerializeField]
        protected string _ID;
        public string ID { get { return _ID; } }
        public string FormatID(string prefix)
        {
            return ID + " " + prefix;
        }

        [SerializeField]
        protected AnimationCurveToggleValue curve;
        public AnimationCurveToggleValue Curve { get { return curve; } }
        protected virtual void SetCurve(float time)
        {
            Rewind.Curves.Set(ID, curve.Evaluate(time));
        }

        public AnimationTriggerRewind Rewind { get; protected set; }

        public virtual void InitComponent(Animator animator)
        {
            if(Rewind == null) Rewind = animator.GetComponent<AnimationTriggerRewind>();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, stateInfo, layerIndex);

            InitComponent(animator);

            if (Rewind != null)
            {
                Rewind.Trigger(FormatID("Start"));

                SetCurve(stateInfo.normalizedTime);
            }
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            InitComponent(animator);

            if (Rewind != null)
            {
                Rewind.Trigger(FormatID("End"));

                SetCurve(0f);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateUpdate(animator, stateInfo, layerIndex);

            InitComponent(animator);

            if (Rewind != null)
            {
                SetCurve(stateInfo.normalizedTime);
            }
        }
    }

    [Serializable]
    public class AnimationCurveToggleValue : ToggleValue<AnimationCurve>
    {
        public virtual float Evaluate(float time)
        {
            if (enabled)
                return value.Evaluate(time);

            return time;
        }
    }
}