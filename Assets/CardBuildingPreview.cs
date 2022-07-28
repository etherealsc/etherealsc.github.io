using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardBuildingPreview : MonoBehaviour
{
    public TMP_InputField monsterName;
    public TMP_InputField monsterCost;
    public TMP_InputField monsterPower;
    public TMP_InputField monsterToughness;
    public TMP_InputField monsterSpeed;
    public TMP_InputField monsterSet;

    public TMP_Dropdown keyword1;
    public TMP_Dropdown keyword2;
    public TMP_Dropdown keyword3;
    public TMP_Dropdown keyword4;

    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI power;
    public TextMeshProUGUI toughness;
    public TextMeshProUGUI speed;
    public TextMeshProUGUI set;
    public TextMeshProUGUI description;

    public List<string> keywordList;

    void Update()
    {
        cardName.text = monsterName.text;
        cost.text = monsterCost.text;
        power.text = monsterPower.text;
        toughness.text = monsterToughness.text;
        speed.text = monsterSpeed.text;

        description.text = keyword1.GetComponentInChildren<TMP_Text>().text.ToString() 
                + "    " + keyword2.GetComponentInChildren<TMP_Text>().text.ToString() 
                + "    " + keyword3.GetComponentInChildren<TMP_Text>().text.ToString() 
                + "    " + keyword4.GetComponentInChildren<TMP_Text>().text.ToString();
    }


    public void MakeNewCard()
    {
        keywordList.Clear();

        if (keyword1.GetComponentInChildren<TMP_Text>().text.ToString() != "")
        {
            keywordList.Add(keyword1.GetComponentInChildren<TMP_Text>().text.ToString());
        }
        if (keyword2.GetComponentInChildren<TMP_Text>().text.ToString() != "")
        {
            keywordList.Add(keyword2.GetComponentInChildren<TMP_Text>().text.ToString());
        }
        if (keyword3.GetComponentInChildren<TMP_Text>().text.ToString() != "")
        {
            keywordList.Add(keyword3.GetComponentInChildren<TMP_Text>().text.ToString());
        }
        if (keyword4.GetComponentInChildren<TMP_Text>().text.ToString() != "")
        {
            keywordList.Add(keyword4.GetComponentInChildren<TMP_Text>().text.ToString());
        }

        if (cardName.text != null && cost.text != null && power.text != null && toughness.text != null && speed.text != null && monsterSet.text != null)
        {
            FirebaseManager.instance.CreateCard(cardName.text, cost.text, power.text, toughness.text, speed.text, monsterSet.text, keywordList);
        }
    }
}
