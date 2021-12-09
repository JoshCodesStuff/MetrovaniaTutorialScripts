using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtRangeState : IEnemyStates
{
    private Enemy enemy;

    public void Enter(Enemy enemy) { Debug.Log("At Range"); ; this.enemy = enemy; }
    public void Exit() { }
    public void Execute() 
    {
        if (enemy.InHitRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        else if (enemy.target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
