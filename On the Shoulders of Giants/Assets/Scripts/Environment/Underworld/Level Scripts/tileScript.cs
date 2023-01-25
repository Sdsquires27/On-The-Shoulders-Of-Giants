using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tileScript : MonoBehaviour
{
    private GameManager gameManager;
    public float gridSizeX;
    public float gridSizeY;
    public int buildScene;
    public placingTile startingTile;
    public List<Level> levels = new List<Level>();
    private Vector2 startingPos;



    // Start is called before the first frame update
    void Start()
    {
        levels.Clear();
        levels.Add(startingTile.level);
        startingPos = startingTile.transform.position;
        startingTile.selectable = false;
        gameManager = FindObjectOfType<GameManager>();
        DontDestroyOnLoad(gameObject);
    }

    private void OnLevelWasLoaded(int level)
    {
        gameManager = FindObjectOfType<GameManager>();
        if (level == buildScene)
        {
            GenerateMap();
        }
    }

    private void GenerateMap()
    {
        int i = 0;
        foreach (Level level in levels)
        {
            GameObject x = Instantiate(level.level);
            x.transform.position += new Vector3(gameManager.mapSizeX * level.pos.x, gameManager.mapSizeY * level.pos.y, 0);
            x.transform.SetParent(transform, true);
            if (x.GetComponent<LevelScript>() != null)
            {
                createLevelObjects(x);
            }

            i++;
        }
    }

    void createLevelObjects(GameObject level)
    {
        LevelScript tempLevel = level.GetComponent<LevelScript>();

        if (tempLevel.hasItem)
        {
            ItemScript it = tempLevel.item;
            it.setItem(tempLevel.rarity);
        }

        if (tempLevel.enemies.Length > 0)
        {
            int[] enemyIndexes = new int[2];

            enemyIndexes[0] = Random.Range(0, tempLevel.enemies.Length);
            enemyIndexes[1] = Random.Range(0, tempLevel.enemies.Length);
            Debug.Log(enemyIndexes[0]);
            Debug.Log(enemyIndexes[1]);

            // if they are equal, chnage the second enemy
            if (enemyIndexes[0] == enemyIndexes[1])
            {
                if (enemyIndexes[1] == tempLevel.enemies.Length - 1)
                {
                    enemyIndexes[1] -= 1;
                }
                else
                {
                    enemyIndexes[1] += 1;
                }
            }


            foreach (int index in enemyIndexes)
            {
                print(index);
                tempLevel.enemies[index].SetActive(true);
            }

        }
    }

    public void tryPlaceTile(placingTile tile)
    {
        Vector2 tilePos = tile.transform.position;
        
        Vector2 simplePos = tilePos - startingPos;

        simplePos = new Vector2((int)(simplePos.x / gridSizeX), (int)(simplePos.y / gridSizeY));
        List<bool> canPlaceTile = new List<bool>(levels.Count);

        /*
        check against each level to make sure the tile can be placed.
        checks first that the tiles x are the same, compares the y to see if they are touching.
        If touching, sees if the actual level has an opening in the direction they are connecting.
        If this is true, the tile can be placed against that other tile. If not, then the tile cannot be placed.
        Does the same thing for if the y are the same. If both are the same it also cannot be placed. 
        */

        foreach (Level lvl in levels)
        {
            if (simplePos.x == lvl.pos.x)
            {
                if (simplePos.y == lvl.pos.y - 1)
                {
                    if (tile.level.north && lvl.south)
                    {
                        canPlaceTile.Add(true);
                        continue;
                    }
                    else
                    {
                        canPlaceTile.Add(false);
                        break;
                    }
                }
                else if (simplePos.y == lvl.pos.y + 1)
                {
                    if (tile.level.south && lvl.north)
                    {
                        canPlaceTile.Add(true);
                        continue;
                    }
                    else
                    {
                        canPlaceTile.Add(false);
                        break;
                    }
                }
                else if (simplePos.y == lvl.pos.y)
                {
                    canPlaceTile.Add(false);
                    break;
                }
            }
            else if (simplePos.y == lvl.pos.y)
            {
                if (simplePos.x == lvl.pos.x - 1)
                {
                    if (tile.level.east && lvl.west)
                    {
                        canPlaceTile.Add(true);
                        continue;
                    }
                    else
                    {
                        canPlaceTile.Add(false);
                        break;
                    }

                }
                else if (simplePos.x == lvl.pos.x + 1)
                {
                    if (tile.level.west && lvl.east)
                    {
                        canPlaceTile.Add(true);
                        continue;
                    }
                    else
                    {
                        canPlaceTile.Add(false);
                        break;
                    }
                }
            }
        }

        // checks that the tile is placeable
        if (canPlaceTile.Contains(true) && !canPlaceTile.Contains(false))
        {
            PlaceTile(tile, tilePos, simplePos);
        }
    }
    private void PlaceTile(placingTile tile, Vector2 tilePos, Vector2 levelPos)
    {
        tile.startingPos = tilePos;
        tile.selectable = false;
        Level level = tile.level;
        level.pos = levelPos;
        levels.Add(level);
    }
}



[System.Serializable]
public class Level
{
    public GameObject level;
    public Vector2 pos;
    public bool north;
    public bool east;
    public bool south;
    public bool west;
}
