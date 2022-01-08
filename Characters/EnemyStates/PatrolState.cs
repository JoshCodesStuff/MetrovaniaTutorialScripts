using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyStates
{
    private Enemy enemy;

    private float patrolTimer;
    private float patrolDuration = 10f;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        Debug.Log("Patrolling");
        patrolTimer = 0f;
        patrolDuration = Random.Range(5, 10);
    }
    public void Execute()
    {
        Patrol();

        enemy.Move();

        if (enemy.target != null && enemy.VisibleRange)
        {
            enemy.ChangeState(new AtRangeState());
        }
    }
    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        if (patrolTimer >= patrolDuration) enemy.ChangeState(new IdleState());
    }
    public void Exit()
    {
        //do nothing
    }
}
