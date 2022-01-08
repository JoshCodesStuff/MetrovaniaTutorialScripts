using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyStates
{
    private float attackTimer;
    private float attackCooldown = 3;
    private bool canAttack = true;

    private Enemy enemy;

    public void Enter(Enemy enemy) 
    {
        Debug.Log("Meleeing");
        this.enemy = enemy;
        if (enemy.VisibleRange && !enemy.InMeleeRange)
        {
            enemy.ChangeState(new AtRangeState());
        }
        else if (enemy.target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }
    public void Execute() 
    { 
        Attack();

        if (enemy.VisibleRange && !enemy.InMeleeRange)
        {
            enemy.ChangeState(new AtRangeState());
        }
        else if (enemy.target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }
    public void Attack()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer>=attackCooldown)
        {
            canAttack = true;
            Debug.Log("Can Attack Now");
            attackTimer = 0;
        }
        if (canAttack)
        {
            canAttack = false;
            Debug.Log("Hitting");
            enemy.anim.SetTrigger("attack");
        }
    }
    public void Exit() 
    {
        //do nothing
    }
}
