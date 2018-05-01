using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtyBit : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<Player>().health--;
                break;
        }
    }
}
