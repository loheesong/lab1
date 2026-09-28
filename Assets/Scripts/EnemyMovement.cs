using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    private float originalX;
    private float maxOffset = 5.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;

    [SerializeField] private Transform spawnPoint;

    void Start() {
        enemyBody = GetComponent<Rigidbody2D>();
        ResetGoomba();
    }

    public void ResetGoomba() {
        transform.position = spawnPoint.position;
        originalX = transform.position.x;
        moveRight = -1; // start moving left 
        ComputeVelocity();
    }
    void ComputeVelocity() {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba() {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // note that this is Update(), which still works but not ideal. See below.
    void Update() {

    }

    void FixedUpdate() {
        if (enemyBody.position.x > originalX + maxOffset) {
            moveRight = -1;
            ComputeVelocity();
        } else if (enemyBody.position.x < originalX - maxOffset) {
            moveRight = 1;
            ComputeVelocity();
        }
        Movegoomba();
    }
}