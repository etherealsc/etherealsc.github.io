using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler//, IDropHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Vector3 startingPosition;
    //public DragProtection dragProtection;

    [SerializeField] private Canvas canvas;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        startingPosition = transform.position;
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    private void Update()
    {
        //if(itemSlot != null)
        //{
        //    gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        //}
        //else
        //{
        //    gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;

        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        startingPosition = transform.position;

        //throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        //throw new System.NotImplementedException();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");

        if (itemSlot.currentEquippedCard != null)
        {
            itemSlot.currentEquippedCard = null;
        }
            //throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        //throw new System.NotImplementedException();
    }

    //public void OnDrop(PointerEventData eventData)
    //{
    //    Debug.Log("d");
    //    //throw new System.NotImplementedException();
    //}

    public ItemSlot itemSlot;
}
