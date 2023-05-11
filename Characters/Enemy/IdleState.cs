using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyStates
{
    private EnemyCharacter enemy;

    private float idleTimer;
    private float idleDuration;
    
    public void Enter(EnemyCharacter enemy)
    {
        this.enemy = enemy;
        Debug.Log("Enemy: I'm Idling");
        idleTimer = 0;
        idleDuration = Random.Range(1, 10);
    }
    public void Execute()
    {
        Idle();
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
    private void Idle()
    {
        enemy.anim.SetFloat("speed", 0);
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
    public void Exit()
    {
        //do nothing
    }
}
