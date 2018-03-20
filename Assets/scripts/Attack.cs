using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    public int threshold;

    private int count = 0;
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        count++;
        //Debug.Log(rb2d.velocity.y + name + this.GetHashCode());

        if (count > threshold)
        {
            //Debug.Log(count);
            Destroy(gameObject);
        }
    }

}
