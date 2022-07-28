using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    void Awake()
    {
        //check that this instance exists
        if (instance == null)
        {
            //if it doesnt, make it exist
            instance = this;
        }
        else if (instance != this)
        {
            //destroy duplicate instances
            Destroy(gameObject);
        }
        //set this instance as protected
        DontDestroyOnLoad(gameObject);




    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CardGameBoard")
        {
            //GameObject.Find("PanelWithAnswer")
            playerDeck = GameObject.Find("PlayerDeck").GetComponent<PlayerDeck>();
            opponentDeck = GameObject.Find("EnemyDeck").GetComponent<PlayerDeck>();

            for (int i = 0; i < 30; i++)
            {
                playerDeck.deck[i] = FirebaseManager.instance.playerDecklist[i];
                opponentDeck.deck[i] = FirebaseManager.instance.opponentDecklist[i];
            }


        }
    }
    public PlayerDeck playerDeck;
    public PlayerDeck opponentDeck;
}
