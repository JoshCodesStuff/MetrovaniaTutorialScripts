using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    private IEnemyStates currentState;

    [SerializeField] public GameObject target { get; set; }
    public Transform player;

    [SerializeField] private float meleeRange;
    [SerializeField] private float visibleRange;
    [SerializeField] private bool dropItem;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [SerializeField] private float distanceToPlayer;

    public bool InMeleeRange
    {
        get
        {
            if (target != null)
            {
                return Vector2.Distance(transform.position, target.transform.position) <= meleeRange;
            }
            return false;
        }
    }
    public bool VisibleRange
    {
        get
        {
            if (target != null)
            {
                return Vector2.Distance(transform.position, target.transform.position) <= visibleRange;
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
            LookAtTarget();
        }
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
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
    public void RemoveTarget()
    {
        target = null;
        ChangeState(new PatrolState());
    }
    public void Move()
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                anim.SetFloat("speed", 1);
                transform.Translate(GetDirection() * (speed * Time.deltaTime));
            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }
    private void LookAtTarget()
    {
        if (target != null)
        {
            float xDir = target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(hitCheck.position, hitRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftEdge.position, 1f);
        Gizmos.DrawWireSphere(rightEdge.position, 1f);
    }
    public override void Death()
    {
        Debug.Log("Enemy Died");
    }
}
