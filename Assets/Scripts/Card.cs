using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Card
{
    public int id;
    public string setCode;
    public string cardName;
    public int cost;
    public int power;
    public int toughness;
    public int speed;
    public List<string> cardKeywords = new List<string>();

    public Sprite cardImage;

    public Color colour;
    public Card()
    {

    }

    public Card(int Id, string SetCode,string CardName, int Cost, int Power, int Toughness, int Speed, List<string> CardKeywords, Sprite CardImage, Color32 Colour)
    {
        id = Id;
        setCode = SetCode;
        cardName = CardName;
        cost = Cost;
        power = Power;
        toughness = Toughness;
        speed = Speed;
        cardKeywords = CardKeywords;
        cardImage = CardImage;
        colour = Colour;
    }

}
