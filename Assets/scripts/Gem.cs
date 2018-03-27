using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour {

    public float speed;
    public Player player;
    public bool big;
    public bool changed;
    public int threshold;
    public float exForce;

    bool inside;
    public float radius = 5f;
    public float force = 100f;
    private int count = 0;
    private Rigidbody2D rb2d;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Vector2 randomDirection = new Vector2(Random.value * exForce, Random.value * exForce);
        rb2d.AddForce(randomDirection);
    }

    private void Update()
    {
        if (inside)
        {
            Vector3 magnetField = player.transform.position - transform.position;
            float index = (radius - magnetField.magnitude) / radius;
            rb2d.AddForce(force * magnetField * index);
        }
    }

    private void LateUpdate()
    {
        count++;
        //Debug.Log(rb2d.velocity.y + name + this.GetHashCode());

        // Won't decay if near the player
        if (count > threshold && !inside)
        {
            //Debug.Log(count);
            Destroy(gameObject);
        }
    }

    // Magnetism
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Magnet":
                inside = true;
                break;

            case "Player":

                // Get parent of projectile - ie player
                GameObject player = collision.gameObject.transform.gameObject;
                Player script = player.GetComponent<Player>();
                script.score++;

                // Big gems give 5
                if (big)
                {
                    script.score += 4;
                }
                Destroy(gameObject);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Magnet")
        {
            inside = false;
            count = 0;
        }

        
    }
}
