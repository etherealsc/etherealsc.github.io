using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerDeck : MonoBehaviour
{

    public List<Card> deck = new List<Card>();

    public int x;




    // Start is called before the first frame update
    void Start()
    {
        //import decklist from GameManager
        //for (int i = 0; i < 30; i++)
        //{
        //    deck[i] = GameManager.chosenDeck[x];


        //}




        x = 0;
        
        //for (int i = 0; i < 30; i++)
        //{
        //    x = Random.Range(0, 9);

        //    deck[i] = CardDatabase.cardList[x];

            
        //}

        Shuffle(deck);
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDeckSize();
    }

    //Fisher-Yates randomization code adapted from
    //https://gist.github.com/jasonmarziani/7b4769673d0b593457609b392536e9f9
    public void Shuffle(List<Card> deck)
    {
        Debug.Log("Pre-Shuffle List");
        for (int i = 0; i < deck.Count; i++)
        {
            Debug.Log(deck[i].cardName);
        }


        for (int i = deck.Count - 1; i > 0; i--)
        {
            // Randomize a number between 0 and i (so that the range decreases each time)
            int rnd = Random.Range(0, i);

            // Save the value of the current i, otherwise it'll overwrite when we swap the values
            Card temp = deck[i];

            // Swap the new and old values
            deck[i] = deck[rnd];
            deck[rnd] = temp;
        }

        Debug.Log("Post-Shuffle List");
        for (int i = 0; i < deck.Count; i++)
        {
            Debug.Log(deck[i].cardName);
        }
    }

    public void PlayerDeckShuffle()
    {
        Shuffle(deck);
    }


    public GameObject fullDeck;
    public GameObject deck80;
    public GameObject deck60;
    public GameObject deck40;
    public GameObject deck20;
    public TextMeshProUGUI count;
    
    public void PlayerDeckSize()
    {
        count.text = deck.Count.ToString();
        //eventually change to respond to event of deck.count changing
        if(deck.Count >= 21)
        {
            fullDeck.SetActive(true);
            deck80.SetActive(true);
            deck60.SetActive(true);
            deck40.SetActive(true);
            deck20.SetActive(true);
        }
        else if (deck.Count >=16)
        {
            fullDeck.SetActive(false);
            deck80.SetActive(true);
            deck60.SetActive(true);
            deck40.SetActive(true);
            deck20.SetActive(true);
        }
        else if (deck.Count >= 11)
        {
            fullDeck.SetActive(false);
            deck80.SetActive(false);
            deck60.SetActive(true);
            deck40.SetActive(true);
            deck20.SetActive(true);
        }
        else if (deck.Count >= 6)
        {
            fullDeck.SetActive(false);
            deck80.SetActive(false);
            deck60.SetActive(false);
            deck40.SetActive(true);
            deck20.SetActive(true);
        }
        else if (deck.Count >= 1)
        {
            fullDeck.SetActive(false);
            deck80.SetActive(false);
            deck60.SetActive(false);
            deck40.SetActive(false);
            deck20.SetActive(true);
        }
        else
        {
            fullDeck.SetActive(false);
            deck80.SetActive(false);
            deck60.SetActive(false);
            deck40.SetActive(false);
            deck20.SetActive(false);
        }
    }

    public GameObject cardTemplate;
    public GameObject playerHand;
    public bool isPlayer;
    public void DrawCard()
    {
        if (deck.Count > 0)
        {
            GameObject card;
            card = Instantiate(cardTemplate, playerHand.transform);
            card.GetComponent<ThisCard>().thisCard = deck[0];

            card.GetComponent<ThisCard>().isCardFaceUp = isPlayer;
            
            deck.Remove(deck[0]);
        }
        else
        {
            //game loss
        }
    }

}
