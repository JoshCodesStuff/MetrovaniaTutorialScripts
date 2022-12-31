using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string CharacterType;
    [field: SerializeField] public bool Attacking { get; set; }
    [field: SerializeField] public bool TakingDamage { get; set; }
    [field: SerializeField] public Animator anim { get; private set; }
    protected abstract bool Dead { get; }
    protected bool facingRight;

    [SerializeField] protected GameObject projectile;

    [Header("Stats")]
    [SerializeField] protected float speed;
    [SerializeField] protected float power;
    [SerializeField] protected Stats healthStat;

    [Header("Attack Details")]
    public float hitRadius;
    public Transform attackCheck;
    public LayerMask whatIsTarget; // thing the character wants to kill

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
        Collider2D target = Physics2D.OverlapCircle(attackCheck.position, hitRadius, whatIsTarget);
        if (target != null)
        {
            Debug.Log(CharacterType + " attacked " + target.GetComponent<Character>().CharacterType);
            StartCoroutine(target.GetComponent<Character>().TakeDamage());
        }
    }

    public virtual void RangedAttack()
    {
        if (facingRight)
        {
            GameObject temp = (GameObject)Instantiate(projectile, attackCheck.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, -90.0f)));
        }
        else
        {
            GameObject temp = (GameObject)Instantiate(projectile, attackCheck.position, Quaternion.Euler(new Vector3(0.0f, 0.0f, +90.0f)));
        }
    }

    public abstract IEnumerator TakeDamage();
    public abstract void Death();
}
