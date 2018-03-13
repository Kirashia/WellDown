using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {

    float range;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(Life());
	}

    IEnumerator Life()
    {
        yield return new WaitForSeconds(range);
        gameObject.SetActive(false);
    }
}
