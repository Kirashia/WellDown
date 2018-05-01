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
    public int score = 0;

    private bool attacking = false;
    private Rigidbody2D rb2d;
    public bool canAttack = false;
    [SerializeField] private int energy;


    // Use this for initialization
    void Start()
    {
        Debug.Log("new234");
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
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
        rb2d.velocity = new Vector2(rb2d.velocity.x, float.Epsilon);
        while (Input.GetKey(KeyCode.Space) && energy > 0)
        {
            rb2d.AddForce(new Vector2(0, recoil));
            //Debug.Log("Attacking");
            energy--;

            // Create the weapon projectile
            GameObject attackInstance = Instantiate(attack, transform.position + new Vector3(0, -1, 0), Quaternion.identity) as GameObject;
            attackInstance.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -attackForce));
            attackInstance.transform.parent = transform;

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

    public void Shout()
    {
        Debug.Log("AOSFIJDSA:LKEHFLA:WKDHLDNLKWHDOSAI");
        Debug.Log("AOSFIJDSA:LKEHFLA:WKDHLDNLKWHDOSA1");
        Debug.Log("AOSFIJDSA:LKEHFLA:WKDHLDNLKWHDOSA2");
        Debug.Log("AOSFIJDSA:LKEHFLA:WKDHLDNLKWHDOSA3");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Exit":
                GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.NewMap();
                break;
        }
    }
}