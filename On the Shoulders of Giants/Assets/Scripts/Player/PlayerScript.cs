using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private int defaultMoveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private int maxJumps;
    [SerializeField] public Weapon weapon;
    [SerializeField] private int maxHealth;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D bottomCol;
    public ItemScript item;
    public DescriptionText itemDescription;

    // private variables
    private GameObject interactable;
    private int moveSpeed;
    private float curMoveInput;
    private int curJumps;
    private Collider2D weaponCol;
    private int curHealth;
    private bool canMove = true;
    private bool inv;

    private void Awake()
    {
        curJumps = maxJumps;
        moveSpeed = defaultMoveSpeed;
    }

    public void changeWeaon(Weapon newWeapon)
    {
        weapon = newWeapon;
    }

    public void changeMaxJumps(int newMax)
    {
        maxJumps = newMax;
    }

    public void changeJumpForce(float newForce)
    {
        jumpForce = newForce;
    }

    public void changeMoveSpeed(int newSpeed)
    {
        moveSpeed = newSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        itemDescription = FindObjectOfType<DescriptionText>();
        item.setItem(1);
        item = null;
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // reset jumps on hitting ground
        if (collision.otherCollider == bottomCol)
        {
            curJumps = maxJumps;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // remove a jump on leaving the ground if given an extra jump before leaving
        if (collision.otherCollider == bottomCol)
        {
            if (curJumps == maxJumps)
            {
                curJumps--;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // show item description text
        if (collision.CompareTag("Item"))
        {
            interactable = collision.gameObject;
            itemDescription.showItemText(transform.position, collision.GetComponent<ItemScript>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // hide item description text
        if (collision.CompareTag("Item"))
        {
            interactable = null;
            itemDescription.stopShowText();
        }
    }

    /*    private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "tilemap")
            {
                if (collision.otherCollider)
            }
        }*/

    private void FixedUpdate()
    {
        move();


        // flips sprite based on velocity
        if (curMoveInput != 0)
        {
            sr.flipX = rb.velocity.x > 0 ? false : true;

        }
    }

    private void move()
    {
        rb.velocity += new Vector2(curMoveInput * moveSpeed, 0);
        rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed) * .8f, rb.velocity.y);
    }

    public void setMaxJumps(int jumps)
    {
        maxJumps = jumps;
    }

    private void jump()
    {
        curJumps--;
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    public void onJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (curJumps > 0)
            {
                jump();
            }
        }
    }

    public void onMoveInput(InputAction.CallbackContext context)
    {
        if (canMove) curMoveInput = context.ReadValue<float>();
        else curMoveInput = 0;
        // sets the move input to the maximum to prevent controller errors
    }

    public void onAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            attack();
        }
    }

    public void onInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (interactable != null)
            {
                // if player presses interact key while touching item, pick up item
                if (interactable.CompareTag("Item"))
                {
                    interactable.GetComponent<ItemScript>().itemPickUp(this);
                }
                else if (interactable.CompareTag("NPC"))
                {
                    // going to need to figure out a way to write this so that it can handle different types of npcs with more complex dialogue options
                    interactable.GetComponent<NPC>().interact();
                }
            }
        }
    }
    public void damage(int damage, Vector2 sourcePos)
    {
        if (!inv)
        {
            curHealth -= damage;
            sourcePos = ((Vector2)transform.position - sourcePos).normalized;
            rb.AddForce(sourcePos * 20, ForceMode2D.Impulse);
            StartCoroutine(stun(.8f));
            StartCoroutine(invincible(1.7f));
        }
    }

    private void attack()
    {
        if (weaponCol == null)
        {
            GameObject tempWeapon = new GameObject();
            // creates a new game object to hold the weapon, so that the tag can be changed for on trigger events
            tempWeapon.transform.parent = transform;
            tempWeapon.transform.position = transform.position;
            tempWeapon.tag = "Player Weapon";

            if (weapon.weaponType == "Fist")
            {
                // generates collider used for weapon
                BoxCollider2D weaponCol = tempWeapon.AddComponent<BoxCollider2D>();
                weaponCol.isTrigger = true;
                weaponCol.size = new Vector2(.5f, .5f);
                moveSpeed /= 2;

                StartCoroutine(moveWeaponFist(weaponCol));
            }
            else if (weapon.weaponType == "Sword")
            {
                SpriteRenderer spriteRenderer = tempWeapon.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = Resources.Load<Sprite>("Sprites/Weapons/Sword");
                tempWeapon.transform.localScale = new Vector3 (sr.flipX ? -1 : 1, 1, 1);
                tempWeapon.transform.position += Vector3.right * (sr.flipX ? -1 : 1);
                tempWeapon.transform.position += Vector3.up * .5f;
                tempWeapon.transform.rotation = Quaternion.Euler(0f, 0f, 50 * (sr.flipX ? -1 : 1));
                weaponCol = tempWeapon.AddComponent<PolygonCollider2D>();
                weaponCol.isTrigger = true;
                
                moveSpeed /= 2;

                StartCoroutine(moveWeaponSword(tempWeapon, spriteRenderer.flipX));
            }
        }

    }

    private IEnumerator moveWeaponFist(Collider2D weaponCol, bool endStun = false)
    {
        for (int i = 0; i < weapon.weaponSpeed; i++)
        {
            // moves collider forward
            weaponCol.offset += Vector2.right / weapon.weaponSpeed * (sr.flipX ? -1 : 1);
            yield return new WaitForSeconds(.1f);
        }
        // destroys both collider and the game object it is attached to once finished
        Destroy(weaponCol.gameObject);
        moveSpeed = defaultMoveSpeed;
    }
    private IEnumerator moveWeaponSword(GameObject sword, bool flipped = false, bool endStun = false)
    {
        Vector3 rotatePos = sword.transform.position - new Vector3(.2f, .2f, 0);
        for (int i = 0; i < weapon.weaponSpeed * 10; i++)
        {
            // rotate the sword around its corner
            // sword.transform.Rotate(new Vector3(0, 0, -18 / weapon.weaponSpeed * (sr.flipX ? -1 : 1)));
            sword.transform.RotateAround(rotatePos, new Vector3(0, 0, 1), -18 / weapon.weaponSpeed * (sr.flipX ? -1 : 1));

            yield return new WaitForSeconds(.01f);
        }
        // destroys both collider and the game object it is attached to once finished
        Destroy(sword);
        moveSpeed = defaultMoveSpeed;
    }

    private IEnumerator stun(float timeToWait)
    {
        print("stun");
        canMove = false;
        yield return new WaitForSeconds(timeToWait);
        canMove = true;
    }

    private IEnumerator invincible(float timeToWait)
    {
        inv = true;
        yield return new WaitForSeconds(timeToWait);
        inv = false;
    }

    bool IsOnGround()
    {
        // sends a ray below the player to see if they're on the ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.8f);
        return hit.collider != null;
    }
}

[System.Serializable]
public class Weapon
{
    public string weaponType;
    public float weaponSpeed;
    public int weaponDmg;
}