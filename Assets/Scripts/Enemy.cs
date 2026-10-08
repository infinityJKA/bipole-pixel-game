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

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float minMoveTime = 0.5f;
    [SerializeField] private float maxMoveTime = 2.0f;
    [SerializeField] private float minWaitTime = 0.5f;
    [SerializeField] private float maxWaitTime = 1.5f;
    private Vector2 moveDirection;
    private bool isMoving;

    void OnEnable()
    {
        currentHP = maxHP;
        player = FindObjectOfType<PlayerMovement>();
    }
    void Start()
    {
        StartCoroutine(WanderRoutine());
    }

    void Update()
    {
        if(invulnerable && Time.time - lastTimeHit >= invulTime)
        {
            invulnerable = false;
            Color currentColor = spriteRenderer.color;
            spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
            StartCoroutine(WanderRoutine());
        }
    }

     void FixedUpdate()
    {
        if (isMoving)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OnTriggerEnter2D enemy");

        if (collision.gameObject.CompareTag("Player Projectile"))
        {
            ProjectileBehavior projectile = collision.gameObject.GetComponent<ProjectileBehavior>();

            
            AudioManager.Instance.PlaySfx("PlayerHitProjectile");

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
            
            AudioManager.Instance.PlaySfx("EnemyHit");
            
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isMoving)
        {
            StopAllCoroutines();
            isMoving = false;
            rb.velocity = Vector2.zero;
            StartCoroutine(WanderRoutine()); // Restart routine immediately to pick a new path
        }
    }

     private IEnumerator WanderRoutine()
    {
        while (true)
        {
            // choose to moe or not
            if (Random.value > 0.25f) 
            {
                moveDirection = cardinalDirections[Random.Range(0, cardinalDirections.Length)];
                isMoving = true;

                float walkDuration = Random.Range(minMoveTime, maxMoveTime);
                yield return new WaitForSeconds(walkDuration);
            }
            
            isMoving = false;
            rb.velocity = Vector2.zero; 

            float waitDuration = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitDuration);
        }
    }


    private readonly Vector2[] cardinalDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.left,
        Vector2.right
    };

    private void TakeDamage(int d, Vector2 k)
    {
        currentHP -= d;

        if (currentHP <= 0)
        {
            Destroy(gameObject);
            return;
        }

        StopAllCoroutines();
        isMoving = false;
        rb.velocity = Vector2.zero;

        rb.AddForce(k, ForceMode2D.Impulse);
        invulnerable = true;
        lastTimeHit = Time.time;

        Color currentColor = spriteRenderer.color;
        spriteRenderer.color = new Color(currentColor.r, currentColor.g, currentColor.b, 0.7f);
    }

}
