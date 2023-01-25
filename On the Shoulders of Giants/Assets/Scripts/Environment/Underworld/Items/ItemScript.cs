using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ItemScript : MonoBehaviour
{
    private SpriteRenderer sr;
    private string type;
    private DirectoryInfo dir = new DirectoryInfo("Assets/Resources/Sprites/Items");
    private Sprite sprite;
    FileInfo[] info;
    private Collider2D col;
    public string description;
    private int rarity;
    int secondaryType;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        string filePath;
        // set up the file directory to be images in the folder specified in the initializer
        info = dir.GetFiles("*.png");

        // gets the names of all of those objects
        List<string> types = new List<string>();
        foreach (FileInfo f in info)
        {
            types.Add(f.Name);
        }
        type = types[Random.Range(0, types.Count)];

        // remove file extension
        string tempType = type.Remove(type.IndexOf("."));

        type = tempType;

        // get the filepath into the folder with different item sprites
        filePath = "Sprites/Items/" + type;

        // create the sprite
        sprite = Resources.Load<Sprite>(filePath);

        // set the sprite
        sr.sprite = sprite;

        // add a new collider and turn it into a trigger
        col = gameObject.AddComponent<PolygonCollider2D>();
        col.isTrigger = true;

    }

    public void setItem(int itemRarity)
    {
        //secondary type is for different types of abilities within an item associated with a certain type
        if (type == "Boot")
        {
            secondaryType = Random.Range(0, 2);
            if (secondaryType == 0)
            {
                description = "These mystical boots provide the wearer with " + (itemRarity + 1).ToString() + " jumps while equipped.";
            }
            else if (secondaryType == 1)
            {
                description = "These strange boots give the wearer to float in the air for a limited amount of time.";
            }
        }
        else if (type == "Sword")
        {
            secondaryType = Random.Range(0, 5);
            string secondaryDescription;

            // dagger
            if (secondaryType == 0)
            {
                if (rarity == 1) secondaryDescription = "put something here";
                description = "";
            }
        }
        else if (type == "Square")
        {
            description = "This item is a square. A square is commonly known as a useless item.";
        }
        else if (type == "Circle")
        {
            description = "This is a circle. What are you gonna do about it?";
        }
    }

    public void itemPickUp(PlayerScript player)
    {
        if (type == "Boot")
        {
            if (secondaryType == 0)
            {
                player.setMaxJumps(rarity + 1);
            }
            else if (secondaryType == 1)
            {
                // uh put something here later when you figure out how these actually work
            }
        }
        else if (type == "Square" || type == "Circle")
        {
            player.changeMoveSpeed(12);
        }
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
