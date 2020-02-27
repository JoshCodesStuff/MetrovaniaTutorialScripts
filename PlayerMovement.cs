using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//component required for script to run
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    //accessible inside editor
    public float speed = 2;
    public float horizMovement;

    //cannot be accessed inside editor
    private Rigidbody2D rb2D;
    private bool facingRight;

    private Animator myAnimator;

    //called on start
    private void Start()
    {
        //define the rigidbody as the scene loads
        rb2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        //direction set [GetAxisRaw returns 0,1 or -1 
        //based on input
        horizMovement = Input.GetAxis("Horizontal");
    }

    //called by internal clock rather than frame updates
    private void FixedUpdate()
    {
        //adds a force to the players rigidbody2d (moves the player)
        rb2D.velocity = new Vector2(horizMovement * speed, rb2D.velocity.y);
        myAnimator.SetFloat("speed", Mathf.Abs(horizMovement));
        Flip(horizMovement);
    }

    private void Flip(float horizontal)
    {
        if (horizontal < 0 && !facingRight || horizontal > 0 && facingRight)
        {
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
}
