using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragProtection : MonoBehaviour, IDropHandler
{
    public GameObject currentlyEquippedCard;

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDropFakeScreen");
        //throw new System.NotImplementedException();

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragAndDrop>().startingPosition;

        }
    }
}