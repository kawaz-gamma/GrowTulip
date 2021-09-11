using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveCycleSetter : StateMachineBehaviour
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //animator.SetFloat("MoveOffset", animator.GetFloat("TimeLeft"));
        animator.SetFloat("MoveOffset", Random.Range(0, 0.5f));
    }
}