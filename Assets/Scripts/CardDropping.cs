using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDropping : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDropFakeScreen");
        //throw new System.NotImplementedException();

        //if (eventData.pointerDrag != null)
        //{
            
        //    //ItemSlot tempSwap = eventData.pointerDrag.GetComponent<DragAndDrop>().itemSlot;

        //    //eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragAndDrop>().startingPosition; if you drop anything on me, go home
        //    eventData.pointerDrag.GetComponent<DragAndDrop>().itemSlot = gameObject.GetComponent<DragAndDrop>().itemSlot; //makes the new word's
        //    eventData.pointerDrag.GetComponent<DragAndDrop>().itemSlot.currentEquippedCard = eventData.pointerDrag;
        //    eventData.pointerDrag.gameObject.transform.SetParent(eventData.pointerDrag.GetComponent<DragAndDrop>().itemSlot.gameObject.transform,false);
        //    //eventData.pointerDrag.GetComponent<RectTransform>().position = eventData.pointerDrag.GetComponent<DragAndDrop>().itemSlot.gameObject.GetComponent<RectTransform>().position;
        //    //eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = eventData.pointerDrag.GetComponent<DragAndDrop>().itemSlot.gameObject.GetComponent<RectTransform>().anchoredPosition;

        //    //gameObject.GetComponent<RectTransform>().anchoredPosition = tempSwap.GetComponent<RectTransform>().anchoredPosition;

        //    //gameObject.GetComponent<DragAndDrop>().itemSlot = tempSwap;
        //    gameObject.GetComponent<DragAndDrop>().itemSlot.currentEquippedCard = gameObject;


        //}
    }

}
