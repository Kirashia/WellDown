using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    private TunnelCreator tunnelCreator;

	// Use this for initialization
	void Start ()
    {
        tunnelCreator = GetComponent<TunnelCreator>();
        tunnelCreator.CreateSineTunnel();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
