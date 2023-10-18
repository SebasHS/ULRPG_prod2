using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateBoss : StateBoss
{
    public IdleStateBoss(BossController controller) : base(controller)
    {
        // Creamos nuestra transicion de Idle -> Follow
        BossTransition transitionIdleToFollow = new BossTransition(
            isValid: () => {
                float distance = Vector3.Distance(
                    controller.Player.position,
                    controller.transform.position
                );
                if (distance < controller.DistanceToFollow)
                {
                    return true;
                }else
                {
                    return false;    
                }
                
            },
            getNextState: () => {
                return new FollowStateBoss(controller);
            }
        );

        Transitions.Add(transitionIdleToFollow);
    }

    public override void OnStart()
    {
        Debug.Log("Estado Idle Start Boss");
        controller.rb.velocity = Vector3.zero;
        controller.animator.SetFloat("Vertical", -1f);
    }

    public override void OnUpdate()
    {
        //Debug.Log("Estado Idle: Update");
    }

    public override void OnFinish()
    {
        Debug.Log("Estado Idle Finish Boss");
    }
}