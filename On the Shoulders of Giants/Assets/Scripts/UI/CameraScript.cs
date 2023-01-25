using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [Header("Settings")]
    public float cameraSpeed;

    // private variables
    private GameManager gameManager;
    private PlayerScript playerScript;
    private Vector3 finalPos;
    private Vector3 lerpPos;
    float newX;
    float newY;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        playerScript = FindObjectOfType<PlayerScript>();
        finalPos = transform.position;
        lerpPos = transform.position;
        float newX = transform.position.x;
        float newY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        checkForMovement();
        lerpPos = Vector3.Lerp(transform.position, finalPos, cameraSpeed);
        transform.position = lerpPos;
    }

    private void checkForMovement()
    { 
        if (playerScript.transform.position.x > transform.position.x + gameManager.mapSizeX / 2)
        {
            newX += gameManager.mapSizeX;
        }
        else if (playerScript.transform.position.x < transform.position.x - gameManager.mapSizeX / 2)
        {
            newX -= gameManager.mapSizeX;
        }
        else if (playerScript.transform.position.y > transform.position.y + gameManager.mapSizeY / 2)
        {
            newY += gameManager.mapSizeY;
        }
        else if (playerScript.transform.position.y < transform.position.y - gameManager.mapSizeY / 2)
        {
            newY -= gameManager.mapSizeY;
        }

        finalPos = new Vector3(newX, newY, transform.position.z);
    }
}
