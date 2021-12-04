using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private Enemy enemy;
    private IEnemyStates currentState;

    [SerializeField] private GameObject target { get; set; }
    [SerializeField] private float hitRange;
    [SerializeField] private bool dropItem;

    private bool InHitRange
    {
        get
        {
            if (target != null)
            {
                return Vector2.Distance(transform.position, target.transform.position) <= hitRange;
            }
            return false;
        }
    }
    protected override bool IsDead
    {
        get
        {
            return healthStat.CurrentVal <= 0;
        }
    }
    public override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
    }
    public void Update()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
        }
    }
    public void ChangeState(IEnemyStates newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter(this);
    }
    public override IEnumerator TakeDamage()
    {
        healthStat.CurrentVal -= 10;

        if (!IsDead)
        {
            anim.SetTrigger("hit");
        }
        else
        {
            anim.SetTrigger("die");
            yield return null;
        }
    }
}
