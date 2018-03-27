using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject playerGO;

    private Player player;
    private TunnelCreator tunnelCreator;

	// Use this for initialization
	void Start ()
    {
        tunnelCreator = GetComponent<TunnelCreator>();
        tunnelCreator.CreateSineTunnel();
        player = playerGO.GetComponent<Player>();

        playerGO.transform.position = new Vector2(4, tunnelCreator.length + 50);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(player.health <= 0)
        {
            Debug.Log("Game over");
        }
	}

    public void NewMap()
    {
        Debug.Log("))");
    }
}
