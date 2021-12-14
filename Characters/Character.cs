using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator anim { get; private set; }
    public bool Attack { get; set; }
    protected bool facingRight;
    protected abstract bool IsDead { get; }
    protected bool TakingDamage { get; set; }

    [SerializeField] protected GameObject projectile;

    [Header("Stats")]
    [SerializeField] protected int health;
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
