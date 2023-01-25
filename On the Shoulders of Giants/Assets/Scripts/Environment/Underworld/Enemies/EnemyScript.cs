using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private int health;
    [SerializeField] private int maxHealth;
    private PlayerScript player;
    [SerializeField] private int dmg;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player Weapon"))
        {
            damage(player.weapon);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            player.damage(dmg, transform.position);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    private void damage(Weapon weapon)
    {
        // only decreased health for now, may add more later, like knockback for bigger weapons.
        health -= weapon.weaponDmg;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
