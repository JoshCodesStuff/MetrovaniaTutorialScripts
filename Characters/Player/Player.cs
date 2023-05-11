using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : Character
{
    [Header("Jump Details")]
    private float JumpForgiveTime;                      // press jump within 'x' time and you jump
    [SerializeField] private float MaxJumpTime;         // max time for jumping
    [SerializeField] private float JumpForce;           // force for jumping
    [SerializeField] private float JumpDiminish;        // amount jump diminishes over time
    [SerializeField] private float MaxJumpForgiveTime;  // the 'x'
    private bool IsFalling
    {
        get
        {
            return rb.velocity.y < 0f;
        }
    }

    [Header("Slide Details")]
    [SerializeField] private float MaxSlideSpeed;
    private float SlideSpeed;
    private Vector2 SlideDir;
    private State state;
    private enum State
    {
        Normal,
        DodgeRollSliding,
    }
    
    [Header("Ground Details")]
    [SerializeField] private LayerMask whatIsGround;            // assigned in the inspector
    [SerializeField] private Transform groundCheck;             // detects touching the ground
    [SerializeField] private float groundCheckRadius;           // radius for ground check
    private float GroundedCounter;                              // counts how long since last grounded
    [field: SerializeField] private bool Grounded { get; set; } // whether player is grounded or not
    

    /* components and instances */
    private static Player instance;
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
    protected override bool Dead 
    { 
        get 
        {
            return healthStat.CurrentVal < 1;
        } 
    }
    public Rigidbody2D rb { get; set; }

    #region monos
    private void Awake()
    {
        state = State.Normal;
    }
    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        
        GroundedCounter = 0f;
        JumpForgiveTime = 0f;
    }
    private void Update()
    {
        switch (state) {
            case State.Normal:
                HandleAnims();
                HandleJumping();
                HandleMovement();
                HandleDodgeRoll();
                break;
            case State.DodgeRollSliding:
                HandleDodgeRollSliding();
                break;
        }
    }
    #endregion //monos
    #region Handles
    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        
        if (!TakingDamage && !Dead)
        {
            if (Attacking && Grounded)
                rb.velocity = Vector2.zero;
            else if (!Grounded)
                rb.velocity = new Vector2(horizontal * speed * 0.5f, rb.velocity.y);
            else
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            Flip(horizontal);
            anim.SetFloat("speed", Mathf.Abs(horizontal));
        }
    }
    private void HandleDodgeRoll () {
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            Debug.Log("ROLL BOIO");
            state = State.DodgeRollSliding;                         // set the state
            SlideDir = facingRight ? Vector2.right : Vector2.left;  // check direction of slide/roll (left | right)
            SlideSpeed = MaxSlideSpeed;                             // reset our roll/sliding speed
        }
    }
    private void HandleDodgeRollSliding ()
    {
        anim.SetTrigger("roll");
        SlideSpeed -= (SlideSpeed * Time.deltaTime);
        Debug.Log(SlideSpeed);
        rb.AddForce(SlideDir * SlideSpeed, ForceMode2D.Impulse);

        if (SlideSpeed < 1f)
            state = State.Normal;
    }
    private void HandleJumping()
    {
        /* Grounded check to see if player is on or near ground */
        Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        /* if we are grounded... */
        if (Grounded)
        {
            GroundedCounter = MaxJumpForgiveTime;
            anim.SetBool("falling", false);
            anim.ResetTrigger("jump");
        }
        else GroundedCounter -= Time.deltaTime; //count down
        
        if (!Dead)
        {
            /* If we press jump... */
            if (Input.GetButtonDown("Jump"))
                JumpForgiveTime = MaxJumpForgiveTime;
            else
                JumpForgiveTime -= Time.deltaTime;

            if ((JumpForgiveTime > 0f) && (GroundedCounter > 0f))
            {
                JumpForgiveTime = 0f;
                GroundedCounter = 0f;

                // animate
                anim.SetTrigger("jump");
                anim.SetLayerWeight(1, 1);

                //jump
                rb.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
            }
            /* if you release the jump button... */
            if (Input.GetButtonUp("Jump"))
            {
                if (IsFalling)
                {
                    anim.SetBool("falling", true);
                }
                else 
                {
                    rb.velocity = new Vector2(rb.velocity.x, JumpDiminish * JumpForce);
                    anim.SetBool("falling", false);
                }
            }
        }
    }
    private void HandleAnims()
    {
        if (!TakingDamage || Dead)
        {
            if (Input.GetKeyDown(KeyCode.X)) anim.SetTrigger("attack");
            if (Input.GetKeyDown(KeyCode.V)) anim.SetTrigger("shoot");
            
            if (IsFalling)
            {
                anim.SetBool("falling", true);
            }
            else
            {
                anim.SetBool("falling", false);
            }
        }

    }
    #endregion
    public void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }
    public override IEnumerator TakeDamage()
    {
        healthStat.CurrentVal--;

        if (!Dead)
        {
            Debug.Log("Player health at " + healthStat.CurrentVal);
            //add camera shake and knockback fx
            anim.SetTrigger("damage");
        }
        else
        {
            anim.SetLayerWeight(0, 0);
            anim.SetTrigger("die");
        }

        yield return null;
    }
    
    /* In development */
    public override void Death()
    {
        Debug.Log("Player Died");

    }
    
    /* Debugging */
    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}
