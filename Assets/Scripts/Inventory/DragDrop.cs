using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private CanvasGroup canvasGroup;
    private Canvas canvas;
    RectTransform rectTransform;
    private GameController gameController;
    private Item item;
    private int activeScene;
    public GameObject ItemObject;

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

    public void SetGameController(GameController newGameController) {
        gameController = newGameController;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        activeScene = gameController.GetActiveScene();

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
        rectTransform.SetParent(GameObject.Find(activeScene.ToString()).GetComponent<RectTransform>());
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        
        InventoryManager.Instance.Remove(item);
        
        GameObject obj = Instantiate(ItemObject, GameObject.Find(activeScene.ToString()).GetComponent<RectTransform>());

        var itemIcon = obj.GetComponent<UnityEngine.UI.Image>(); 
        obj.GetComponent<RectTransform>().localPosition = rectTransform.localPosition;
        
        itemIcon.sprite = item.icon;
        obj.GetComponent<ItemPickup>().Item = item;
        obj.GetComponent<ItemController>().Item = item;
        
        Destroy(rectTransform.gameObject);
    }

}
