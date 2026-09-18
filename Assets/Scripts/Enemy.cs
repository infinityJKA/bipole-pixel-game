using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] BoxCollider2D hurtBox;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] SpriteRenderer spriteRenderer;
    float lastTimeHit = 0;
    float invulTime = 0.2f;
    bool invulnerable = false;
    public int maxHP, currentHP;
    public PlayerMovement player;

    void OnEnable()
    {
        currentHP = maxHP;
        player = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        if(invulnerable && Time.time - lastTimeHit >= invulTime)
        {
            invulnerable = false;
            Color currentColor = spriteRenderer.color;
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D enemy");

        if (collision.gameObject.CompareTag("Player Projectile"))
        {
            ProjectileBehavior projectile = collision.gameObject.GetComponent<ProjectileBehavior>();

            if (invulnerable)
            {
                Destroy(projectile.gameObject);
                return;
            }

            TakeDamage((int)projectile.damage, projectile.knockback);
            Destroy(projectile.gameObject);
        }
        else if (collision.gameObject.CompareTag("PlayerMelee"))
        {
            if(invulnerable)
            {
                return;
            }

            Vector2 knockback;

            if(player.facing == 0f) //DOWN
            {
                knockback = new Vector2(0, -player.meleeKnockback);
            }
            else if(player.facing == 0.1f) //LEFT
            {
                knockback = new Vector2(-player.meleeKnockback, 0);
            }
            else if(player.facing == 0.2f) //UP
            {
                knockback = new Vector2(0, player.meleeKnockback);
            }
            else //RIGHT
            {
                knockback = new Vector2(player.meleeKnockback, 0);
            }

            TakeDamage(player.meleeDamage, knockback);


        }


    }

    private void TakeDamage(int d, Vector2 k)
    {
        currentHP -= d;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            return;
        }

        rb.AddForce(k, ForceMode2D.Impulse);
        invulnerable = true;
        lastTimeHit = Time.time;

        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.7f);
    }

}
