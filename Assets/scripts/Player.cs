using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float attackForce;
    public float something;
    public bool jumping = false;
    public float waitForSeconds;
    public float weaponCooldown;
    public int weaponEnergy = 8;

    public GameObject attack;

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
        rb2d.velocity = new Vector2(x, rb2d.velocity.y);


        // Jump
        if (rb2d.velocity.y == 0)
        {
            jumping = false;
            canAttack = false;
            energy = weaponEnergy;
            Debug.Log("Not Jumping");
        }

        if (!jumping && Input.GetKeyDown(KeyCode.Space))
        {
            jumping = true;
            Debug.Log("Jumping");
            StartCoroutine(Jump());
        }


        if (jumping && Input.GetKey(KeyCode.Space) && canAttack && energy > 0)
        {
            StartCoroutine(Attack());
        }
        else if (energy == 0)
        {
            Debug.Log("empty");
        }
    }

    IEnumerator Attack()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
        rb2d.AddForce(new Vector2(0, attackForce));
        Debug.Log("Attacking");
        energy--;

        // Create the weapon projectile
        Instantiate(attack, transform.position + new Vector3(0, -1, 0), Quaternion.identity);

        yield return new WaitForSeconds(weaponCooldown);
    }

    IEnumerator Jump()
    {
        canAttack = false;
        rb2d.AddForce(new Vector2(0, jumpForce));
        yield return new WaitForSeconds(waitForSeconds);
        Debug.Log("no");
        canAttack = Input.GetKeyUp(KeyCode.Space);
    }
}