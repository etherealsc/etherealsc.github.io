using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{
    public TMP_Text monsterNameText;
    public TMP_Text powerText;
    public TMP_Text toughnessText;
    public TMP_Text speedText;
    public TMP_Text keywordsText;


    public void NewScoreElement(string monsterName, string power, string toughness, string speed, string keywords)
    {
        monsterNameText.text = monsterName;
        powerText.text = power.ToString();
        toughnessText.text = toughness.ToString();
        keywordsText.text = keywords.ToString();
    }

}
