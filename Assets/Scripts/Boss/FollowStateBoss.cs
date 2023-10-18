using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowStateBoss : StateBoss
{
    public FollowStateBoss(BossController controller) : base(controller)
    {
        // Transicion Follow -> Idle
        BossTransition transitionFollowToIdle = new BossTransition(
            isValid: () => {
                float distance = Vector3.Distance(
                    controller.Player.position,
                    controller.transform.position
                );
                if (distance >= controller.DistanceToFollow)
                {
                    return true;
                }else
                {
                    return false;    
                }
            },
            getNextState: () => {
                return new IdleStateBoss(controller);
            }
        );
        Transitions.Add(transitionFollowToIdle);

        // Transicion Follow -> Attack
        BossTransition transitionFollowToAttack = new BossTransition(
            isValid: () =>{
                float distance = Vector3.Distance(
                    controller.Player.position,
                    controller.transform.position
                );
                if (distance < controller.DistanceToAttack)
                {
                    return true;
                }else
                {
                    return false;
                }
            },
            getNextState: () => {
                return new AttackStateBoss(controller);
            }
        );
        Transitions.Add(transitionFollowToAttack);
    }


    public override void OnStart()
    {
        Debug.Log("Estado Follow Boss: Start");
    }

    public override void OnUpdate()
    {
        //Debug.Log("Estado Follow: Update");
        Vector3 dir = (
            controller.Player.position - controller.transform.position
        ).normalized;
        controller.animator.SetFloat("Horizontal", dir.x);
        controller.animator.SetFloat("Vertical", dir.z);
        controller.rb.velocity = dir * controller.Speed;

    }
    public override void OnFinish()
    {
        Debug.Log("Estado Follow Boss: FInish");
    }
}
