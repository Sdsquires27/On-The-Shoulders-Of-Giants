using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemy : EnemyScript
{
    public float targetX;
    private float startingX;
    public float moveSpeed;
    private SpriteRenderer sr;

    private bool toTarget;

    private void Start()
    {
        startingX = transform.position.x;
        sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {

        transform.position += new Vector3(moveSpeed * (toTarget ? 1 : -1) * Time.deltaTime, 0, 0);
        if (transform.position.x < startingX)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            toTarget = true;
        }
        else if (transform.position.x > targetX)
        {
            toTarget = false;
            transform.localScale = new Vector3(1, 1, 1);
        }

    }
}
