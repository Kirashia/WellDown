using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public GameObject player;
    public float speed;
    public float health;
    public GameObject bigGem;
    public GameObject smallGem;
    public int worth;

    private Player playerScript;
    private Transform playerTransform;


    // Use this for initialization
    void Start () {
        playerScript = player.GetComponent<Player>();
        playerTransform = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, step);

        if(health <= 0)
        {
            // Create gems on death
            Gemify();
            Destroy(gameObject);
        }
    }

    void Gemify()
    {
        int noBig = worth / 5;
        int noSmall = worth % noBig;

        for (int b = 0; b < noBig; b++)
        {
            Instantiate(bigGem, transform.position, Quaternion.identity);
        }

        for (int s = 0; s < noSmall; s++)
        {
            Instantiate(smallGem, transform.position, Quaternion.identity);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Projectile":
                health--;
                //// Get parent of projectile - ie player
                //GameObject player = collision.gameObject.transform.parent.gameObject;
                //Player script = player.GetComponent<Player>();
                //script.score++;
                break;
        }
    }
}
