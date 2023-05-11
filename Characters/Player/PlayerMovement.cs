using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private PlayerCharacter PC;
    [SerializeField] private float speed;
    [SerializeField] private bool Grounded { get; set; }

    public bool IsFalling
    {
        get
        {
            return rb.velocity.y < 0f;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PC = GetComponent<PlayerCharacter>();
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (!PC.TakingDamage && !PC.Dead)
        {
            if (PC.Attacking && PC.Grounded)
                rb.velocity = Vector2.zero;
            else if (!PC.Grounded)
                rb.velocity = new Vector2(horizontal * speed * 0.5f, rb.velocity.y);
            else
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            Flip(horizontal);
            anim.SetFloat("speed", Mathf.Abs(horizontal));
        }
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !PC.facingRight || horizontal < 0 && PC.facingRight)
        {
            PC.ChangeDirection();
        }
    }
}