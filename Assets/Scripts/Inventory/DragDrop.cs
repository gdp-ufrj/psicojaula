using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    Vector3 originalPosition;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    Transform parentAfterDrag;
    RectTransform rectTransform;

    private Item item;

    private void Awake() {
        canvas = GameObject.Find("canvasBtn").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void AddItem(Item newItem) {
        
        item = newItem;
    }

    public Item GetItem() {
        
        return item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        //parentAfterDrag = transform.parent;
        //transform.SetParent(transform.root);

    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log(canvas.scaleFactor);

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        //rectTransform.anchoredPosition = originalPosition;
    }

}
