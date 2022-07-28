using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject currentEquippedCard;

    private void Update()
    {
        if(currentEquippedCard != null)
        {
            gameObject.GetComponent<Image>().raycastTarget = false;

        }
        else
        {
            gameObject.GetComponent<Image>().raycastTarget = true;

        }
    }

    public void CardRaycastTesting()
    {
        currentEquippedCard.GetComponent<CanvasGroup>().blocksRaycasts = !currentEquippedCard.GetComponent<CanvasGroup>().blocksRaycasts;

    }
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        //throw new System.NotImplementedException();

        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.transform.position = gameObject.transform.position;

            if (currentEquippedCard == null)
            {
                Debug.Log("adding words");

                currentEquippedCard = eventData.pointerDrag;

                currentEquippedCard.GetComponent<DragAndDrop>().itemSlot = gameObject.GetComponent<ItemSlot>();
                currentEquippedCard.transform.SetParent(this.gameObject.transform);
                currentEquippedCard.GetComponent<CanvasGroup>().blocksRaycasts = false;
            }
            else
            {
                eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragAndDrop>().startingPosition;

                //Debug.Log("Attempting to swap words");
                //currentEquippedCard.GetComponent<RectTransform>().position = currentEquippedCard.GetComponent<DragAndDrop>().startingPosition;
                //currentEquippedCard = eventData.pointerDrag;
                //currentEquippedCard.GetComponent<DragAndDrop>().itemSlot = gameObject.GetComponent<ItemSlot>();

            }
        }
    }
}
