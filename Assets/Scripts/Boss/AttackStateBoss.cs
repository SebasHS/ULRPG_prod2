using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateBoss : StateBoss
{
    private float timer = 0.0f;
    public AttackStateBoss(BossController controller) : base(controller)
    {
        // Attack -> Follow
        BossTransition transitionAttackToFollow = new BossTransition(
            isValid : () => {
                float distance = Vector3.Distance(
                    controller.Player.position,
                    controller.transform.position
                );
                if (distance >= controller.DistanceToAttack)
                {
                    return true;
                }else
                {
                    return false;
                }
            },
            getNextState : () => {
                return new FollowStateBoss(controller);
            }
        );

        Transitions.Add(transitionAttackToFollow);
    }


    public override void OnStart()
    {
        Debug.Log("Estado Attack Boss: Start");
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer > controller.CoolDownTime)
        {
            controller.Fire();
            Debug.Log("FIRE????");
            timer = 0f;
        }
    }
    public override void OnFinish()
    {
        Debug.Log("Estado Attack Boss: Finish");
    }
}