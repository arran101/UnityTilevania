using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    Rigidbody2D bulletRigidbody;
    PlayerMovementScript player;
    float xSpeed;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovementScript>();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    
    void Update()
    {
        bulletRigidbody.velocity = new Vector2 (xSpeed, 0f); 
    }

    //for killing enemy if bullet hits
    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }

    //for destroying bullet if it hits anything
    void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
    }
}
