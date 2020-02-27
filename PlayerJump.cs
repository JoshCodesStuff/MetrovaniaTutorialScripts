using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Details")]
    public float jumpTime;//max time for jumping
    public float jumpForce;//force for jumping
    private float jumpTimeCounter;//counter tracks time jumping
    private bool stoppedJumping;//tracks when jump ends

    [Header("Ground Details")]
    public LayerMask whatIsGround;//assigned in the inspector
    public Transform groundCheck;//detects touching the ground
    public float groundCheckRadius;//radius for ground check
    private bool grounded;//grounded - y/n

    [Header("Components")]
    private Rigidbody2D rb;//used to apply forces to player
    private Animator myAnimator;
    

    private void Start()
    {
        //sets the jumpCounter to whatever we set our jumptime to in the editor
        jumpTimeCounter = jumpTime;
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //determines bool, grounded, is true or false by seeing if our groundcheck overlaps something on the ground layer
        grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);

        //if we are grounded...
        if(grounded)
        {
            //the jumpcounter is whatever we set jumptime to in the editor.
            jumpTimeCounter = jumpTime;
            //myAnimator.SetLayerWeight 
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            //and u r on the ground...
            if (grounded)
            {
                //jump!
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                stoppedJumping = false;
            }
        }
        //if you keep holding down the mouse button...
        if ((Input.GetKey(KeyCode.W)) && !stoppedJumping)
        {
            //and your counter hasn't reached zero...
            if (jumpTimeCounter > 0)
            {
                //keep jumping!
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        //if you stop holding down the mouse button...
        if (Input.GetKeyUp(KeyCode.W))
        {
            //stop jumping, set counter = zero.
            //timer will reset once we touch the ground
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(groundCheck.position, groundCheckRadius);
    }
}
