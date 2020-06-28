using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public BoxCollider2D playerCollider;
    public Rigidbody2D playerRB;
    public float walkSpeed = 4;
    public bool isPuzzling = false;                      //affects if the movement or puzzle controls activate

    void Awake()
    {
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
        playerCollider = transform.GetComponent<BoxCollider2D>();
        playerRB = transform.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //check for if isPuzzling
      
    }

    void FixedUpdate()
    {
        if (!isPuzzling)
        {
            Move();
        }

        else
        {
            Puzzling();
        }

    }

    public void Move()
    {
        //if has trying to move
        if (Input.anyKey)
        {
            //up and down controls
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                playerRB.velocity = Vector2.up * walkSpeed;
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                playerRB.velocity = Vector2.down * walkSpeed;
            }
        
            //left and right controls 
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                playerRB.velocity = new Vector2(-walkSpeed, playerRB.velocity.y);
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                playerRB.velocity = new Vector2(walkSpeed, playerRB.velocity.y);
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
        }

        //brakes for playermovement
        else
        {
            playerRB.velocity = new Vector2(0, 0);
        }
    }

    public void Puzzling()
    {

    }
}
