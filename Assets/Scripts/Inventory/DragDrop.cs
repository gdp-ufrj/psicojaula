using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    private CanvasGroup canvasGroup;
    private Canvas canvas;
    RectTransform rectTransform;
    private GameController gameController;
    private Item item;
    private int activeScene;
    public GameObject ItemObject;

    private Image itemImg;

    private void Awake()
    {
        canvas = GameObject.Find("canvasBtn").GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        itemImg = transform.GetChild(0).gameObject.GetComponent<Image>();
    }

    public void AddItem(Item newItem)
    {
        item = newItem;
    }

    public Item GetItem()
    {

        return item;
    }

    public void SetGameController(GameController newGameController)
    {
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

        Debug.Log(item.id);
        DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
        ListaItems.Instance.listaItenslargados.Add(itemInventory);
        InventoryManager.Instance.Remove(itemInventory);

        GameObject obj = Instantiate(ItemObject, GameObject.Find(activeScene.ToString()).GetComponent<RectTransform>());
        obj.tag = "ItemDropped";

        var itemIcon = obj.GetComponent<UnityEngine.UI.Image>();
        obj.GetComponent<RectTransform>().localPosition = rectTransform.localPosition;
        obj.GetComponent<RectTransform>().sizeDelta = rectTransform.sizeDelta;

        itemIcon.sprite = item.icon;
        obj.GetComponent<ItemPickup>().Item = item;
        obj.GetComponent<ItemController>().Item = item;
        obj.GetComponent<DialogueTrigger>().SetVariables(dialogueTrigger);

        Destroy(rectTransform.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData){
        if (eventData.button == PointerEventData.InputButton.Left) {    //Examinando o item
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerExamDialogue(false);
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {    //Usando o item
            if (item.isUsable) {
                if (item.id == 8)   //Rem�dio
                    SoundController.GetInstance().PlaySound("tomando_remedio");

                Debug.Log("Usou o item: " + item.name);
                DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
                ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
                InventoryManager.Instance.Remove(itemInventory);
                Destroy(rectTransform.gameObject);
            }
            else
                Debug.Log("O item " + item.name + " n�o � us�vel");   //Som?
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        Color newColor = itemImg.color;
        newColor.a = 0.95f;
        itemImg.color = newColor;
    }
    public void OnPointerExit(PointerEventData eventData) {
        Color newColor = itemImg.color;
        newColor.a = 1;
        itemImg.color = newColor;
    }
}
