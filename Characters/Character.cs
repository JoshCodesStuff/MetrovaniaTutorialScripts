using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public bool Attack { get; set; }
    public bool TakingDamage { get; set; }
    public Animator anim { get; private set; }
    protected bool facingRight;
    protected abstract bool IsDead { get; }

    [SerializeField] protected GameObject projectile;

    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected Stats healthStat;

    [Header("Attack Details")]
    public float hitRadius;
    public Transform hitCheck;
    public LayerMask whatisenemy;

    public virtual void Start()
    {
        Attack = false;
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
        Collider2D player = Physics2D.OverlapCircle(hitCheck.position, hitRadius, whatisenemy);
        if (player != null) StartCoroutine(player.GetComponent<Character>().TakeDamage());
    }

    public virtual void RangedAttack()
    {
        if (facingRight)
        {
            GameObject temp = (GameObject)Instantiate(projectile, hitCheck.position, Quaternion.Euler(new Vector3(0, 0, -90)));
        }
        else
        {
            GameObject temp = (GameObject)Instantiate(projectile, hitCheck.position, Quaternion.Euler(new Vector3(0, 0, +90)));
        }
    }


    public abstract IEnumerator TakeDamage();
    public abstract void Death();
}
