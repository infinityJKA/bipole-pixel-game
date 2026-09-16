using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] BoxCollider2D hurtBox;
    float lastTimeHit = 0;
    float invulTime = 0.2f;
    int maxHP, currentHP;


    private void OnCollisionEnter(Collision collision)
    {
        
         if (collision.gameObject.CompareTag("something"))
        {
            ///
        }

    }

}
