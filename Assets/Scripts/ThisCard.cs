using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class ThisCard : MonoBehaviour
{
    public Card thisCard;
    public int thisId;

    public int id;
    public string setCode;
    public string cardName;
    public int cost;
    public int basePower;
    public int baseToughness;
    public int baseSpeed;
    public List<string> cardDescription = new List<string>();

    public int currentPower;
    public int currentSpeed;
    public int currentToughness;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI powerText;
    public TextMeshProUGUI toughnessText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI descriptionText;

    public Sprite thisSprite;
    public Image theImage;

    public Image cardFrame;

    public GameObject cardBack;
    public bool isCardFaceUp;
    // Start is called before the first frame update
    void Start()
    {
        //thisCard = CardDatabase.cardList[thisId]; //replace "CardDatabase.cardList[thisId]" with a reference to the firebase
        ResetCard();
    }

    // Update is called once per frame
    void Update()
    {
        

        if(isCardFaceUp == true)
        {
            cardBack.SetActive(false);
        }
        else
        {
            cardBack.SetActive(true);
        }
    }

    public void LoadImage()
    {
        FirebaseManager.instance.LoadTestImage(id, theImage, setCode);
    }


    public void DebugCardFlip()
    {
        isCardFaceUp = !isCardFaceUp;
    }

    public void ResetCard()
    {
        cardDescription.Clear();
        descriptionText.text = "";
        id = thisCard.id;
        setCode = thisCard.setCode;
        cardName = thisCard.cardName;
        basePower = thisCard.power;
        baseToughness = thisCard.toughness;
        baseSpeed = thisCard.speed;
        for (int i = 0; i < thisCard.cardKeywords.Count; i++)
        {
            cardDescription.Add(thisCard.cardKeywords[i]);
        }
        cost = thisCard.cost;
        thisSprite = thisCard.cardImage;

        nameText.text = cardName;
        costText.text = cost.ToString();
        powerText.text = basePower.ToString();
        toughnessText.text = baseToughness.ToString();
        speedText.text = baseSpeed.ToString();
        //descriptionText.text = cardDescription;
        for (int i = 0; i < cardDescription.Count; i++)
        {
            if(i+1 == cardDescription.Count)
            {
                descriptionText.text += cardDescription[i];
            }
            else
            {
                descriptionText.text += cardDescription[i] + ", ";

            }
        }

        theImage.sprite = thisSprite;
        //theImage.color = Color.red;

        cardFrame.color = thisCard.colour;
    }
}
