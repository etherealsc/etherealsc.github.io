using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurnManager : MonoBehaviour
{
    public bool isPlayerTurn;

    public int energyCap;
    public int currentEnergy;
    public TextMeshProUGUI energyCounter;

    public TurnSequence turnSequence;
    public enum TurnSequence
    {
        BEGINGAME,
        WHOSONFIRST,
        PLAYERDRAW,
        PLAYERSTANDBY,
        PLAYERMAINPHASE,
        PLAYERBATTLEPHASE,
        PLAYERENDPHASE,

        ENEMYDRAW,
        ENEMYSTANDBY,
        ENEMYMAINPHASE,
        ENEMYBATTLEPHASE,
        ENEMYENDPHASE,

        PLAYERHANDSELECTION,
        PLAYERFIELDSELECTION,
        PLAYERGRAVEYARDSELECTION
    }

    private void Start()
    {
        turnSequence = TurnSequence.BEGINGAME;
    }


    public PlayerDeck playerDeck;
    public PlayerDeck enemyDeck;

    public List<GameObject> playerZones;
    public List<GameObject> enemyZones;



    private void Update()
    {
        switch(turnSequence)
        {
            case (TurnSequence.BEGINGAME):
                //players draw 5 cards
                break;

            case (TurnSequence.WHOSONFIRST):
                //flip a coin, winner picks
                break;

            case (TurnSequence.PLAYERDRAW):
                playerDeck.DrawCard();
                break;

            case (TurnSequence.PLAYERSTANDBY):
                for (int i = 0; i < playerZones.Count; i++)
                {
                    //playerZones[i].GetComponent<CardHolder>().card.standbyEffect();
                    //something like that
                }
                break;

            case (TurnSequence.PLAYERMAINPHASE):
                //highlight cards that can be played or have effects triggered, enable button to go to battle phase
                break;

            case (TurnSequence.PLAYERBATTLEPHASE):
                //only interactions allowed are fights
                break;

            case (TurnSequence.PLAYERENDPHASE):
                //check if effects expire, change state to enemydraw
                break;

            case (TurnSequence.PLAYERHANDSELECTION):
                //for abilities like 'discard one card'
                //remember previous state
                break;

            case (TurnSequence.PLAYERFIELDSELECTION):
                //for targeting effects
                //remember previous state

                break;

            case (TurnSequence.PLAYERGRAVEYARDSELECTION):
                //for monster reborn or similar
                //remember previous state

                break;


            case (TurnSequence.ENEMYDRAW):
                enemyDeck.DrawCard();
                break;

            case (TurnSequence.ENEMYSTANDBY):
                break;

            case (TurnSequence.ENEMYMAINPHASE):
                break;

            case (TurnSequence.ENEMYBATTLEPHASE):
                break;

            case (TurnSequence.ENEMYENDPHASE):
                break;
        }
    }
}
