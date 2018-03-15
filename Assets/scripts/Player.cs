using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float something;
    public bool jumping = true;
    public float waitForSeconds;

    public float health = 4;
    public float invFrames = 1;
    public bool dmgAble = true;

    public GameObject attack;
    public float attackForce;
    public float weaponRange;
    public float weaponCooldown;
    public float recoil;
    public int weaponEnergy = 8;

    private bool attacking = false;
    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2d;
    public bool canAttack;
    [SerializeField] private int energy;


    // Use this for initialization
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float y = Input.GetAxis("Vertical") * Time.deltaTime;
        canAttack = Mathf.Abs(rb2d.velocity.y) > 0;
        rb2d.velocity = new Vector2(x, rb2d.velocity.y);


        // Jump
        if (rb2d.velocity.y == 0)
        {
            jumping = false;
            canAttack = false;
            energy = weaponEnergy;
            Debug.Log("Not Jumping");
        }

        if (jumping && Input.GetKeyDown(KeyCode.Space) && canAttack && energy > 0 && !attacking)
        {
            StartCoroutine(Attack());
        }
        else if (energy == 0)
        {
            Debug.Log("empty");
        }

        if (!jumping && Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            Debug.Log("Jumping");
            StartCoroutine(Jump());
        }

    }

    IEnumerator Jump()
    {
        jumping = true;
        rb2d.AddForce(new Vector2(0, jumpForce));
        Debug.Log(rb2d.velocity);
        do
        {
            yield return null;
        }
        while (rb2d.velocity.y != 0);

        jumping = false;
    }

    IEnumerator Attack()
    {
        attacking = true;
        //rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        while (Input.GetKey(KeyCode.Space) && energy > 0)
        {
            rb2d.AddForce(new Vector2(0, recoil));
            Debug.Log("Attacking");
            energy--;

            // Create the weapon projectile
            GameObject attackInstance = Instantiate(attack, transform.position + new Vector3(0, -1, 0), Quaternion.identity) as GameObject;
            attackInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -attackForce));

            yield return new WaitForSeconds(weaponCooldown);
        }

        attacking = false;
    }

    IEnumerator TakeDamage(int dmg)
    {
        dmgAble = false;
        health--;
        yield return new WaitForSeconds(invFrames);
        dmgAble = true;
    }
}