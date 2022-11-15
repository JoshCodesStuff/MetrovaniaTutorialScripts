using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : Character
{
    private IEnemyStates currentState;

    [SerializeField] public GameObject Target { get; set; }
    public Transform player;

    [SerializeField] private bool dropItem;
    [SerializeField] private float meleeRange;
    [SerializeField] private float visibleRange;
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private float distanceToPlayer;

    [SerializeField] private bool immortal;
    private float immortalDur = 0.1f;
    private SpriteRenderer spRenderer;

    private Rigidbody2D rb;



    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }
    public bool VisibleRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= visibleRange;
            }
            return false;
        }
    }
    protected override bool bDead
    {
        get
        {
            return healthStat.CurrentVal <= 0;
        }
    }
    
    #region monos
    public override void Start()
    {
        base.Start();
        ChangeState(new IdleState());
        rb = GetComponent<Rigidbody2D>();
    }
    public void Update()
    {
        if (!bDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
        distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
    }
    #endregion //monos
    public void ChangeState(IEnemyStates newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentState.Enter(this);
    }
    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }
    public void Move()
    {
        if (!Attacking && !bDead)
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
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }
    
    /**
     * inherited from Character
     */
    private IEnumerator IndicateImmortal()
    {
        while(immortal)
        {
            spRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }
    public override IEnumerator TakeDamage()
    {
        if (!immortal && !bDead)
        {
            if (Random.Range(0, 10) == 0)
            {
                healthStat.CurrentVal -= 3;
                Vector2 knockback = new Vector2(-1 * GetDirection().x * power, Mathf.Abs(power));
                rb.AddForce(knockback, ForceMode2D.Impulse);
                Debug.Log("Enemy takes a Critical Hit");
            }
            else {
                Vector2 knockback = new Vector2(distanceToPlayer * 0.5f * power, Mathf.Abs(power));
                rb.AddForce(knockback, ForceMode2D.Impulse);
                healthStat.CurrentVal--;
            }
        }

        if (!bDead)
        {
            anim.SetTrigger("hit");
            Debug.Log("Enemy health at " + healthStat.CurrentVal);
            Target = Player.Instance.gameObject;
            immortal = true;
            StartCoroutine(IndicateImmortal());
            yield return new WaitForSeconds(immortalDur);
            immortal = false;
        }
        else anim.SetTrigger("die");
        yield return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, hitRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftEdge.position, 1f);
        Gizmos.DrawWireSphere(rightEdge.position, 1f);
    }
    public override void Death()
    {
        Debug.Log("Enemy Died");
    }
}
