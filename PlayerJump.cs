using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerJump : MonoBehaviour
{
    [Header("Jump Details")]
    public float jumpTime;//max time for jumping
    public float jumpForce;//force for jumping
    public bool stoppedJumping;//tracks when jump ends
    public float jumpTimeCounter;//counter tracks time jumping

    [Header("Ground Details")]
    public LayerMask whatIsGround;//assigned in the inspector
    public bool grounded;//grounded - y/n
    public Transform groundCheck;//detects touching the ground
    public float groundCheckRadius;//radius for ground check

    [Header("Rigidbody")]
    private Rigidbody2D rb;//used to apply forces to player

    private void Start()
    {
        //sets the jumpCounter to whatever we set our jumptime to in the editor
        jumpTimeCounter = jumpTime;
        rb = gameObject.GetComponent<Rigidbody2D>();
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
        }
    }

    private void FixedUpdate()
    {
        //if you press down the mouse button...
        if(Input.GetKeyDown(KeyCode.W))
        {
            //and on ground...
            if(grounded)
            {
                  //jump!
                  rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
                  stoppedJumping = false;
            }
        }
        //if you keep holding down the mouse button...
        if((Input.GetKeyDown(KeyCode.W)) && !stoppedJumping)
        {
            //and your counter hasn't reached zero...
            if(jumpTimeCounter > 0)
            {
                //keep jumping!
                rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
                jumpTimeCounter -= Time.deltaTime;
            }
        }
        //if you stop holding down the mouse button...
        if(Input.GetKeyDown(KeyCode.W))
        {
            //stop jumping, set counter = zero.
            //timer will reset once we touch the ground
            jumpTimeCounter = 0;
            stoppedJumping = true;
        }
    }
}
