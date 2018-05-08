using UnityEngine;
using System.Collections;


public class Loader : MonoBehaviour
{
    public GameObject gameManager;          //GameManager prefab to instantiate.
    public GameObject player;         //SoundManager prefab to instantiate.

    public bool testMap;
    public bool testPlayer;

    void Awake()
    {
        if (Player.instance == null && !testMap)
        {
            //Instantiate gameManager prefab
            GameObject p = Instantiate(player) as GameObject;
            p.name = "Player";
            DontDestroyOnLoad(p);
        }
        //Check if a GameManager has already been assigned to static variable GameManager.instance or if it's still null
        if (GameManager.instance == null && !testPlayer)
        {
            //Instantiate gameManager prefab
            GameObject gm = Instantiate(gameManager) as GameObject;
            gm.name = "Game Manager";
            DontDestroyOnLoad(gm);
        }
    }
}