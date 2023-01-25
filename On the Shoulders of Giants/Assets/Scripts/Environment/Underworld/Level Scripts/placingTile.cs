using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placingTile : MonoBehaviour
{
    private bool selected;
    [System.NonSerialized] public Vector3 startingPos;
    [System.NonSerialized] public bool selectable = true;
    private Camera cam;
    private tileScript tiler;

    public Level level;

    private void OnMouseDown()
    {
        if (selectable)
        {
            if (!selected)
            {
                selected = true;
            }
            else
            {
                tiler.tryPlaceTile(this);
            }
        }

    }

    float RoundToNearestGrid(float pos, float gridSize)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }

    // Start is called before the first frame update
    void Start()
    {
        tiler = FindObjectOfType<tileScript>();
        startingPos = transform.position;
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!selectable)
        {
            selected = false;
        }
        if (selected)
        {
            transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(
                RoundToNearestGrid(transform.position.x, tiler.gridSizeX),
                RoundToNearestGrid(transform.position.y, tiler.gridSizeY),
                0);
        }
        else
        {
            transform.position = startingPos;
        }
    }
}