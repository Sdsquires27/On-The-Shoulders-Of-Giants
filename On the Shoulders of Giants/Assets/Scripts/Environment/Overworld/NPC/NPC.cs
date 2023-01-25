using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float xMin;
    [SerializeField] private float xMax;
    [SerializeField] private int minWaitTime;
    [SerializeField] private int maxWaitTime;
    [SerializeField] private int minSpeed;
    [SerializeField] private int maxSpeed;
    [SerializeField] private string[] dialogue;

    private SpriteRenderer sr;
    private float xTarget;
    private float speed;
    private bool dirRight;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        startNewTarget();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            setDarkness(.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            setDarkness(1f);
        }
    }

    public void interact()
    {
        
    }

    void startNewTarget()
    {
        // set new target position
        xTarget = Random.Range(xMin, xMax);

        // set the direction the player is going in
        dirRight = xTarget > transform.position.x;

        // flip the sprite based on direction
        sr.flipX = !dirRight;

        // set the speed randomly
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void setDarkness(float darkness)
    {
        // Sets color on darkness. should be a decimal between 0 and 1
        Color color = new Color(darkness, darkness, darkness);
        sr.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (xTarget != transform.position.x)
        {
            transform.position += new Vector3(speed * (dirRight ? 1 : -1) * Time.deltaTime, 0, 0);
            // if moving right and past the target
            if (dirRight && transform.position.x > xTarget)
            {
                StartCoroutine(waitAndRestart(Random.Range(minWaitTime, maxWaitTime)));
            }
            // if moving left and past the target
            else if (!dirRight && transform.position.x < xTarget)
            {
                StartCoroutine(waitAndRestart(Random.Range(minWaitTime, maxWaitTime)));
            }
        }
    }

    private IEnumerator waitAndRestart(int time)
    {
        // set position equal to transform to prevent further movement
        transform.position = new Vector3(xTarget, transform.position.y, transform.position.z);
        yield return new WaitForSeconds(time);

        // set new random target and direction
        startNewTarget();

    }

}
