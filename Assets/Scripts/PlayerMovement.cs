using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    public float facing = 0;
    Vector2 movement;
    public ProjectileBehavior smallCardUp;
    public Transform upProjectileOffset;

    void Update()
    {
        /*
            Movement Code
        */
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if(movement.x != 0 || movement.y != 0){
            if(movement.y < 0){
                facing = 0f; //DOWN
            }
            else if(movement.y > 0){
                facing = 0.2f; //UP
            }
            else if(movement.x > 0){
                facing = 0.3f; //RIGHT
            }
            else{
                facing = 0.1f; //LEFT
            }
        }

        // if(facing == 0){
        //     Debug.Log("DOWN");
        // }else if(facing == 0.1f){
        //     Debug.Log("LEFT");
        // }else if(facing == 0.2f){
        //     Debug.Log("UP");
        // }else{Debug.Log("RIGHT");}

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetFloat("Facing", facing);

        /*
            Projectile Code
        */

        if(Input.GetButtonDown("Fire1")){
            if(facing == 0.2f){
                Instantiate(smallCardUp,upProjectileOffset);
            }
        }


    }

    void FixedUpdate()
    {
         rb.MovePosition(rb.position+movement.normalized*moveSpeed*Time.fixedDeltaTime);
    }
}
