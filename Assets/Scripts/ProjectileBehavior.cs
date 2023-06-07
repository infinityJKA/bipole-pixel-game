using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    public Vector2 dir;
    public float speed;
    public Rigidbody2D rb;
    public float existTime = 2;
    public float existDistance = 10;
    private float creationTime; 
    public float damage = 1;

    void Awake(){
        creationTime = Time.time;
    }
    void Start(){
        // [Insert shooting sfx here]
    }
    void Update(){
        if(Time.time >= creationTime+existTime){
            Debug.Log("DESTROY via TIME");
            Destroy(gameObject);
        }
        if(Vector3.Distance(transform.position, transform.parent.position) >= existDistance){
            Debug.Log("DESTROY via DISTANCE");
            Destroy(gameObject);
        }
    }
    void FixedUpdate(){
        rb.MovePosition(rb.position+dir.normalized*speed*Time.fixedDeltaTime);
    } 

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Enemy" ) {
            Destroy(gameObject);
        }
    }
}
