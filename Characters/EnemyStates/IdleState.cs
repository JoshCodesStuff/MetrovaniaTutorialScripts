using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyStates
{
    private Enemy enemy;
    [SerializeField] private float idleTimer;
    [SerializeField] private float idleDuration = 3f;
    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
        Debug.Log("I'm Idling");
        idleTimer = 0f;
    }
    public void Execute()
    {
        Idle();
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

    }
}
