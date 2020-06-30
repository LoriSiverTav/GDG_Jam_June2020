using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public BoxCollider2D playerCollider;
    public Rigidbody2D playerRB;
    public float walkSpeed = 4;
    public float keySpeed = 2;
    public static bool isPuzzling = false;                      //affects if the movement or puzzle controls activate
    public static bool isInRange = false;                       //is checked if close to chest/puzzle

    void Awake()
    {
        playerCollider = transform.GetComponent<BoxCollider2D>();
        playerRB = transform.GetComponent<Rigidbody2D>();
        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        //check for if isPuzzling
        if (Input.GetKeyDown(KeyCode.E) && isInRange)
        {
            isPuzzling = !isPuzzling;
        }
    }

    void FixedUpdate()
    {
        if (!isPuzzling)
        {
            Move();
        }
    }

    public void Move()
    {
        //if has trying to move
        if (Input.anyKey)
        {
           playerRB.velocity = new Vector2(0, 0);

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
}
