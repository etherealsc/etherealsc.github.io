using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OpponentDeckStorage : MonoBehaviour
{
    public string deckID;

    public TMP_InputField inputDeckID;
    public void LevelSelect()
    {
        FirebaseManager.instance.GetFixedDeckButton(deckID);
    }

    public void LevelSelectInput()
    {
        if (inputDeckID.text.Length == 5)
        {
            FirebaseManager.instance.StartCoroutine(FirebaseManager.instance.LoadDecksForGame(inputDeckID.text));
        }
    }
}
