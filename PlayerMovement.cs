using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//component required for script to run
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //accessible inside editor
    public float speed = 2;

    private float horizMovement;

    //cannot be accessed inside editor
    private Rigidbody2D rb;

    //called on start
    private void Start()
    {
        //define the rigidbody before scene loads
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    //called by internal clock rather than frame updates
    private void FixedUpdate()
    {
        //direction set [GetAxisRaw returns 0,1 or -1 
        //based on input
        horizMovement = Input.GetAxisRaw("Horizontal");

        //creates a directional vector for our movement
        Vector2 movement = new Vector2(horizMovement, 0);

        //adds a force to the players rigidbody2d (moves the player)
        rb.AddForce(movement * speed);
    }
}
