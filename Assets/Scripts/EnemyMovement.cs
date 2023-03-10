using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;

    Rigidbody2D EnemyRigidBody;

    void Start()
    {
        EnemyRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        EnemyRigidBody.velocity = new Vector2 (moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) {
        moveSpeed = -moveSpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing() {
        transform.localScale = new Vector2 (-(Mathf.Sign(EnemyRigidBody.velocity.x)), 1f);
    }
}
