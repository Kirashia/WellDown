using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject playerGO;
    public static GameManager instance = null;

    private Player player;
    private TunnelCreator tunnelCreator;

	// Use this for initialization
	void Start ()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // if not, set instance to this
            instance = this;
        }
        // If instance already exists and it's not this:
        else if (instance != this)
        {
            // Then destroy this
            Destroy(gameObject);
        }

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

    public void EndGame()
    {
        Debug.Log("End game");
        NewMap();
    }

    public void NewMap()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
