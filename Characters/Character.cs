using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public Animator anim { get; private set; }
    protected bool facingRight;
    protected bool TakingDamage { get; set; }
    public bool Attack { get; set; }
    protected abstract bool IsDead { get; }
    [SerializeField] protected float speed;
    [SerializeField] protected Stats healthStat;


    public virtual void Start()
    {
        Attack = false;
        facingRight = true;
        anim = GetComponent<Animator>();
    }
    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector2(transform.localScale.x * -1, 1);
    }

    public abstract IEnumerator TakeDamage();
}
