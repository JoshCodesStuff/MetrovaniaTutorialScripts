using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]

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
    [SerializeField] private Vector3 distanceToPlayer;

    [SerializeField] private bool immortal;
    private float immortalDur = 0.3f;
    private SpriteRenderer spRenderer;

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
    protected override bool Dead
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
        spRenderer = GetComponent<SpriteRenderer>();
        ChangeState(new IdleState());
    }
    public void Update()
    {
        if (!Dead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
        distanceToPlayer.x = player.transform.position.x - transform.position.x;
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
        if (!Attacking && !Dead)
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
        if (!immortal && !Dead)
        {
            if (Random.Range(0, 10) == 0)
            {
                healthStat.CurrentVal -= 3;
                Debug.Log("Enemy takes a Critical Hit");
            }
            else healthStat.CurrentVal--;
        }

        if (!Dead)
        {
            anim.SetTrigger("hit");
            Target = Player.Instance.gameObject; // for when ranged attacks become a thing

            // prevent attack spamming
            immortal = true;
            StartCoroutine(IndicateImmortal());
            yield return new WaitForSeconds(immortalDur);

            immortal = false;

            // just to keep track of what was happening
            Debug.Log("Enemy health at " + healthStat.CurrentVal);
        }
        else anim.SetTrigger("die");
        yield return null;
    }
    public override void Death()
    {
        GetComponent<Collider2D>().enabled = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackCheck.position, hitRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(leftEdge.position, 1f);
        Gizmos.DrawWireSphere(rightEdge.position, 1f);
    }
}
