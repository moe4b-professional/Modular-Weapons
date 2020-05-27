using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolField : StateMachineBehaviour
{
    [SerializeField]
    protected string _ID;
    public string ID { get { return _ID; } }

    [SerializeField]
    protected bool value;
    public bool Value { get { return value; } }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.SetBool(ID, value);
    }
}
