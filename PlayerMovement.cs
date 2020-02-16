using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//component required for script to run
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //accessible inside editor
    public float speed = 2;
    public float horizMovement;

    //cannot be accessed inside editor
    private Rigidbody2D rb2D;

    //called on start
    private void Start()
    {
        //define the rigidbody as the scene loads
        rb2D = GetComponent<Rigidbody2D>();
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
    }

    private void Flip()
    {
        
    }
}
