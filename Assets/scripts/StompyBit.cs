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
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,500));
                parent.GetComponent<Enemy>().health--;
                break;
        }
    }
}
