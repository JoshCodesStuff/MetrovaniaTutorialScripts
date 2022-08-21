using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]

public class Player : Character
{
    [Header("Jump Details")]
    [SerializeField]private float fJumpTime;//max time for jumping
    [SerializeField]private float fJumpForce;//force for jumping
    private float fJumpTimeCounter;//counter tracks time jumping
    private float fJumpDiminish = 0.35f;
    [SerializeField] private float fJumpPressedRemember = 0f;
    [SerializeField] private float fJumpPressedRememberTime = 0.05f;
    public bool stoppedJumping;//tracks when jump ends
    private bool IsFalling
    {
        get
        {
            return rb.velocity.y < 0;
        }
    }
    
    [Header("Ground Details")]
    [SerializeField] private LayerMask whatIsGround;//assigned in the inspector
    [SerializeField] private Transform groundCheck;//detects touching the ground
    [SerializeField] private float groundCheckRadius;//radius for ground check
    private float fGroundedRemember = 0f;
    private float fGroundedRememberTime = 0.2f;
    public bool grounded;

    [Header("Components")]
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
    protected override bool bDead 
    { 
        get 
        {
            return healthStat.CurrentVal < 1;
        } 
    }
    public Rigidbody2D rb { get; set; }//used to apply forces to player

    public override void Start()
    {
        base.Start();
        fJumpTimeCounter = fJumpTime;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!TakingDamage || bDead)
        {
            HandleAnims();
        }
        
        HandleJumping();
    }
    private void FixedUpdate()
    {
        if (!TakingDamage && !bDead)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            Flip(horizontal);
            anim.SetFloat("speed", Mathf.Abs(horizontal));
        }
    }
    
    private void HandleJumping()
    {
        /* Grounded check to see if player is on or near ground */
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (!grounded) anim.SetLayerWeight(1, 1);

        /* if we are grounded... */
        fGroundedRemember -= Time.deltaTime;
        if (grounded)
        {
            fGroundedRemember = fGroundedRememberTime;
            fJumpTimeCounter = fJumpTime;
            anim.SetBool("falling", false);
            anim.ResetTrigger("jump");
        }

        /* If we press jump... */
        fJumpPressedRemember -= Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }

        if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))
        {
            fJumpPressedRemember = 0;
            fGroundedRemember = 0;
            stoppedJumping = false;
            
            anim.SetTrigger("jump");
            
            rb.velocity = new Vector2(rb.velocity.x, fJumpForce);
        }
        /* if you stop holding down the jump button... */
        if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, fJumpDiminish * fJumpForce);
                fJumpTimeCounter = 0f;
                stoppedJumping = true;
            }
            if (rb.velocity.y < 0)
                anim.SetBool("falling", true);
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            anim.SetTrigger("attack");
        }
        if (IsFalling) anim.SetBool("falling", true);
        else anim.SetBool("falling", false);
    }

    public override IEnumerator TakeDamage()
    {
        healthStat.CurrentVal--;

        if (!bDead)
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
        Gizmos.DrawWireSphere(attackCheck.position, hitRadius);
    }
}
