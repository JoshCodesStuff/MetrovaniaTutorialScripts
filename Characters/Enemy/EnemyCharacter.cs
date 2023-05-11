using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]

public class EnemyCharacter : Character
{
    private IEnemyStates currentState;

    public GameObject Target { get; set; }
    private SpriteRenderer spRenderer;
    public Transform player;

    [SerializeField] private bool dropItem;
    [SerializeField] private bool immortal;
    [SerializeField] private Vector3 distanceToPlayer;

    private float immortalDur = 0.3f;

    public EnemyMovement movement;
    public EnemyCombat attack;

    #region monos
    public override void Start()
    {
        base.Start();
        spRenderer = GetComponent<SpriteRenderer>();
        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<EnemyCombat>();
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
        while (immortal)
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
            Target = PlayerCharacter.Instance.gameObject; // for when ranged attacks become a thing

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