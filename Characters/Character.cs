using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public List<KeyValuePair<string, bool>> Conditions;
    public bool Attacking { get; set; }
    public bool TakingDamage { get; set; }
    public Animator anim { get; private set; }
    protected bool facingRight;
    protected abstract bool bDead { get; }

    [SerializeField] protected GameObject projectile;

    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected Stats healthStat;

    [Header("Attack Details")]
    public float hitRadius;
    public Transform attackCheck;
    public LayerMask whatIsEnemy;

    public virtual void Start()
    {
        Attacking = false;
        facingRight = true;
        anim = GetComponent<Animator>();
        healthStat.Initialise();
    }
    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
    }

    public void MeleeAttack()
    {
        Collider2D player = Physics2D.OverlapCircle(attackCheck.position, hitRadius, whatIsEnemy);
        if (player != null) StartCoroutine(player.GetComponent<Character>().TakeDamage());
    }

    public virtual void RangedAttack()
    {
        if (facingRight)
        {
            GameObject temp = (GameObject)Instantiate(projectile, attackCheck.position, Quaternion.Euler(new Vector3(0, 0, -90)));
        }
        else
        {
            GameObject temp = (GameObject)Instantiate(projectile, attackCheck.position, Quaternion.Euler(new Vector3(0, 0, +90)));
        }
    }


    public abstract IEnumerator TakeDamage();
    public abstract void Death();
}
