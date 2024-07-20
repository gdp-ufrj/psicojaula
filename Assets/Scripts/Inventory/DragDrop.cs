using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler {

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
        DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
        InventoryManager.Instance.Remove(itemInventory);
        
        GameObject obj = Instantiate(ItemObject, GameObject.Find(activeScene.ToString()).GetComponent<RectTransform>());
        obj.tag = "ItemDropped";

        var itemIcon = obj.GetComponent<UnityEngine.UI.Image>(); 
        obj.GetComponent<RectTransform>().localPosition = rectTransform.localPosition;
        
        itemIcon.sprite = item.icon;
        obj.GetComponent<ItemPickup>().Item = item;
        obj.GetComponent<ItemController>().Item = item;
        obj.GetComponent<DialogueTrigger>().SetVariables(dialogueTrigger);
        
        Destroy(rectTransform.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData) {
        DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        if (dialogueTrigger != null)
            dialogueTrigger.TriggerExamDialogue();
    }

}
