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
    public float distance;
    public bool aggro;

    private Transform playerTransform;


    // Use this for initialization
    void Start ()
    {
        player = GameObject.Find("Player");
        playerTransform = player.transform;
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 playerPos = player.transform.position;
        Vector3 pos = transform.position;
        distance = Vector3.SqrMagnitude(playerPos - pos);

        if (distance > 100)
        {
            UnsetAggro();
        }
        else if (distance <= 100)
        {
            SetAggro();
        }

        if (aggro)
        {
            // Move
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(pos, playerPos, step);
        }

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
        int noSmall = worth % 5;

        for (int b = 0; b < noBig; b++)
        {
            Instantiate(bigGem, transform.position, Quaternion.identity);
        }

        for (int s = 0; s < noSmall; s++)
        {
            Instantiate(smallGem, transform.position, Quaternion.identity);
        }

    }

    void SetAggro()
    {
        aggro = true;
    }

    void UnsetAggro()
    {
        aggro = false;
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
