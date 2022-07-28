using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();

    //this will be replaced by importing from the FireBase database when that works
    private void Awake()
    {
        //cardList.Add(new Card(0, "Name", 0, 0, 0, 0, "Description"));
        //Card(int Id, string CardName, int Cost, int Power, int Toughness, int Speed, string CardDescription)
        //cardList.Add(new Card(000, "Knight", "Alpha", 1, 2, 4, 1, new string[] { "Last Stand", "Humanoid", "Dragonslayer" }, Resources.Load<Sprite>("VOID"),new Color32(116,122,121,255)));
        //cardList.Add(new Card(001, "Wizard", "Alpha", 1, 3, 2, 3, new string[] { "Long-Range", "Humanoid", "Mage"}, Resources.Load<Sprite>("VOID"), new Color32(39, 64, 207, 255)));
        //cardList.Add(new Card(002, "Skeleton", "Alpha", 1, 1, 3, 2, new string[] { "Rebuild", "Undead"}, Resources.Load<Sprite>("VOID"), new Color32(255, 0, 0, 255)));
        //cardList.Add(new Card(003, "Dragon", "Alpha", 2, 5, 5, 2, new string[] { "Dragon", "Flying", "Plantslayer" }, Resources.Load<Sprite>("VOID"), new Color32(0, 255, 0, 255)));
        //cardList.Add(new Card(004, "Treant", "Alpha", 1, 3, 6, 1, new string[] { "Healer", "Plant", "Undeadslayer"}, Resources.Load<Sprite>("VOID"), new Color32(0, 0, 255, 255)));
        //cardList.Add(new Card(005, "Witch", "Alpha", 2, 5, 3, 4, new string[] { "Mageslayer", "Mage" }, Resources.Load<Sprite>("VOID"), new Color32(255, 255, 0, 255)));
        //cardList.Add(new Card(006, "Lich", "Alpha", 3, 4, 8, 3, new string[] { "Necromancer", "Mage", "Undead" }, Resources.Load<Sprite>("VOID"), new Color32(255, 0, 255, 255)));
        //cardList.Add(new Card(007, "Gravedigger", "Alpha", 1, 0, 5, 2, new string[] { "Exhume", "Humanoid"}, Resources.Load<Sprite>("VOID"), new Color32(0, 255, 255, 255)));
        //cardList.Add(new Card(008, "Goblin", "Alpha", 1, 2, 2, 2, new string[] { "Swarm", "Goblin", "Humanoidslayer"}, Resources.Load<Sprite>("VOID"), new Color32(255, 255, 255, 255)));
    }
}
