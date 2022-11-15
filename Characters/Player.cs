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
    [SerializeField] private float fJumpDiminish = 0.15f;
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
    private float fGroundedRemember;
    private float fGroundedRememberTime = 0.2f;
    public bool grounded;

    [Header("Weapon Details")]
    [SerializeField] private bool wDrawn;


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

    #region monos
    public override void Start()
    {
        base.Start();
        fJumpTimeCounter = fJumpTime;
        rb = GetComponent<Rigidbody2D>();
        wDrawn = false;
        fGroundedRemember = 0f;
    }
    private void Update()
    {
        if (!TakingDamage || bDead)
        {
            HandleAnims();
        }
        if (!bDead) HandleJumping();
    }
    private void FixedUpdate()
    {
        HandleMovement();
    }
    #endregion //monos

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        
        if (!TakingDamage && !bDead)
        {
            if (Attacking && grounded)
                rb.velocity = Vector2.zero;
            else
                rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

            Flip(horizontal);
            anim.SetFloat("speed", Mathf.Abs(horizontal));
        }
    }
    
    private void HandleJumping()
    {
        /* Grounded check to see if player is on or near ground */
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        /* if we are grounded... */
        if (grounded)
        {
            fGroundedRemember = fGroundedRememberTime;
            fJumpTimeCounter = fJumpTime;
            anim.SetBool("falling", false);
            anim.ResetTrigger("jump");
        }
        else { 
            fGroundedRemember -= Time.deltaTime; //count down
        }

        /* If we press jump... */
        if (Input.GetButtonDown("Jump"))
            fJumpPressedRemember = fJumpPressedRememberTime;
        else
            fJumpPressedRemember -= Time.deltaTime;

        if ((fJumpPressedRemember > 0f) && (fGroundedRemember > 0f))
        {
            fJumpPressedRemember = 0f;
            fGroundedRemember = 0f;
            stoppedJumping = false;
            
            anim.SetTrigger("jump");
            
            rb.AddForce(new Vector2(0, fJumpForce), ForceMode2D.Impulse);
        }
        /* if you stop holding down the jump button... */
        if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, fJumpDiminish * fJumpForce);
                //rb.AddForce(new Vector2(0, fJumpForce), ForceMode2D.Impulse);
                fJumpTimeCounter = 0f;
                stoppedJumping = true;
            }
            if (rb.velocity.y < 0f)
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
