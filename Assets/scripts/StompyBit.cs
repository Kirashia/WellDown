using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompyBit : MonoBehaviour {

    GameObject parent;
    
    // Use this for initialization
	void Start () {
        parent = transform.parent.gameObject;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
                body.velocity = new Vector2(body.velocity.x, 0);
                body.AddForce(new Vector2(0,500));
                parent.GetComponent<Enemy>().health--;
                break;
        }
    }
}
