using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using System;

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 5f;
    public int currentHP, maxHP;
    public float meleeKnockback;
    public int meleeDamage;
    public Rigidbody2D rb;
    public Animator animator;
    public float facing = 0;
    Vector2 movement;
    public ProjectileBehavior smallCardUp, smallCardDown, smallCardLeft, smallCardRight;
    public Transform upProjectileOffsetL, upProjectileOffsetR;
    public Transform downProjectileOffsetL, downProjectileOffsetR;
    public Transform leftProjectileOffsetL, leftProjectileOffsetR;
    public Transform rightProjectileOffsetL, rightProjectileOffsetR;


    public SpriteRenderer spriteRenderer;
    public float tookDamage;
    public bool stunned;

    void Start()
    {
        currentHP = maxHP;
        stunned = false;
    }



    void Update()
    {

        if (stunned)
        {
            if(Time.time - tookDamage > 0.5f)
            {
                Color currentColor = spriteRenderer.color;
                spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
                stunned = false;
            }
            else
            {
                return;
            }
        }


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

        if(Input.GetButtonDown("FireL")){ // LEFT HAND FIRE
            if(facing == 0.2f){ // FACING UP
                Instantiate(smallCardUp,upProjectileOffsetL);
            }else if(facing == 0f){ // FACING DOWN
                Instantiate(smallCardDown,downProjectileOffsetL);
            }else if(facing == 0.1f){ // FACING LEFT
                Instantiate(smallCardLeft,leftProjectileOffsetL);
            }else{ // FACING RIGHT
                Instantiate(smallCardRight,rightProjectileOffsetL);
            }
        }

        else if(Input.GetButtonDown("FireR")){ // RIGHT HAND FIRE

            animator.SetTrigger("Slicing");

            // if(facing == 0.2f){ // FACING UP
            //     Instantiate(smallCardUp,upProjectileOffsetR);
            // }else if(facing == 0f){ // FACING DOWN
            //     Instantiate(smallCardDown,downProjectileOffsetR);
            // }else if(facing == 0.1f){ // FACING LEFT
            //     Instantiate(smallCardLeft,leftProjectileOffsetR);
            // }else{ // FACING RIGHT
            //     Instantiate(smallCardRight,rightProjectileOffsetR);
            // }


        }


    }

    void FixedUpdate()
    {
        if(stunned) return;

         rb.MovePosition(rb.position+movement.normalized*moveSpeed*Time.fixedDeltaTime);
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyHitbox"))
        {
            TakeDamage(collision.gameObject.GetComponent<EnemyHitbox>().damage,collision.gameObject.GetComponent<EnemyHitbox>().knockback);    
        }     
    }

    void TakeDamage(int d, float k)
    {
        currentHP -= d;
        if(currentHP <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            Vector2 knockback;

            if(facing == 0f) //DOWN
            {
                knockback = new Vector2(0, k);
            }
            else if(facing == 0.1f) //LEFT
            {
                knockback = new Vector2(k, 0);
            }
            else if(facing == 0.2f) //UP
            {
                knockback = new Vector2(0, -k);
            }
            else //RIGHT
            {
                knockback = new Vector2(-k, 0);
            }

            rb.velocity = Vector3.zero;

            tookDamage = Time.time;
            Color currentColor = spriteRenderer.color;
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.5f);
            stunned = true;

            rb.AddForce(knockback, ForceMode2D.Impulse);
        }
    }

}
