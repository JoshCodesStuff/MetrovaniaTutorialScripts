using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : Character
{
    [Header("Jump Details")]
    [SerializeField]private float jumpTime;//max time for jumping
    [SerializeField]private float jumpForce;//force for jumping
    private float jumpTimeCounter;//counter tracks time jumping
    private bool stoppedJumping;//tracks when jump ends

    [Header("Ground Details")]
    [SerializeField] private LayerMask whatIsGround;//assigned in the inspector
    [SerializeField] private Transform groundCheck;//detects touching the ground
    [SerializeField] private float groundCheckRadius;//radius for ground check
    public bool grounded;//grounded - y/n
    private bool IsFalling
    {
        get
        {
            return rb.velocity.y < 0;
        }
    }

    [Header("Components")]
    private static Player instance;
    public Rigidbody2D rb { get; set; }//used to apply forces to player
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
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
        jumpTimeCounter = jumpTime;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!TakingDamage || IsDead)
        {
            HandleAnims();
        }
        //if we are grounded...
        if (grounded)
        {
            //jumpcounter is whatever we set jumptime to
            jumpTimeCounter = jumpTime;
        }
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        HandleJumping();
    }
    private void FixedUpdate()
    {
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            Flip(horizontal);
            HandleLayers();
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
            anim.SetFloat("speed", Mathf.Abs(horizontal));
        }
    }
    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            //jump!
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            stoppedJumping = false;
            anim.SetTrigger("jump");
        }
        //if you keep holding down the mouse button...
        if ((Input.GetButton("Jump")) && !stoppedJumping)
        {
            //and your counter hasn't reached zero...
            if (jumpTimeCounter > 0)
            {
                //keep jumping!
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        //if you stop holding down the jump button...
        if (Input.GetButtonUp("Jump"))
        {
            //stop jumping, set counter = zero.
            //timer will reset once we touch the ground
            jumpTimeCounter = 0;
            stoppedJumping = true;
            anim.ResetTrigger("jump");
        }
    }
    public void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }
    private void HandleAnims()
    {
        if (IsFalling) anim.SetBool("falling", true);
        else anim.SetBool("falling", false);
    }
    private void HandleLayers()
    {
        if (!grounded) anim.SetLayerWeight(1, 1);
        else anim.SetLayerWeight(1, 0);
    }
    public override IEnumerator TakeDamage()
    {
        healthStat.CurrentVal--;
        TakingDamage = true;

        if (!IsDead) anim.SetTrigger("damage");
        else
        {
            anim.SetLayerWeight(1, 0);
            anim.SetTrigger("die");
        }
        yield return null;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
    public override void Death()
    {
        Debug.Log("Player Died");

        healthStat.CurrentVal = healthStat.MaxVal;
    }
}
