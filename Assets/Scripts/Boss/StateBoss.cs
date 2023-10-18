using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBoss
{
    protected BossController controller;
    public List<BossTransition> Transitions;
    
    public StateBoss(BossController controller)
    {
        this.controller = controller;
        Transitions = new List<BossTransition>();    
    }

    public abstract void OnStart();
    public abstract void OnUpdate();
    public abstract void OnFinish();

}
