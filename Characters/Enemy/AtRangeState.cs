using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtRangeState : IEnemyStates
{
    private EnemyCharacter enemy;

    public void Enter(EnemyCharacter enemy) 
    { 
        Debug.Log("Enemy: Moving closer");
        this.enemy = enemy; 
    }
    public void Execute()
    {
        //if within melee distance to target
        if (enemy.InMeleeRange) enemy.ChangeState(new MeleeState());

        //else if player is visible but outside of hit range
        else if (enemy.Target != null) enemy.Move();

        //else if enemy can't see player, reset to idle
        else enemy.ChangeState(new IdleState()); 
    }
    public void Exit()
    { 
        //do nothing
    }
}
