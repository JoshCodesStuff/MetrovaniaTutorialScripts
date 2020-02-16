using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump : MonoBehaviour
{
    public float fallMultiplier = 2f;
    public float lowJumpMultiplier = 2f;

    Rigidbody2D rb2D;

    void Awake ()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (rb2D.velocity.y < 0)//if we are falling
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1);
        }
        else if (rb2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1);
        }
    }
}
