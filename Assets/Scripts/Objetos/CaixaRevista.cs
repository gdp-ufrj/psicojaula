using UnityEngine;
using UnityEngine.EventSystems;

public class CaixaRevista : MonoBehaviour, IDropHandler, IPointerClickHandler {

    private RectTransform rectTransform;
    public Item revista;
    public Transform Cena;
    public GameObject Item;
    private bool isOpen = false;

    private void Awake() {
        isOpen = ListaItems.Instance.caixaRevistaIsOpen;
    }

    public void OnDrop(PointerEventData eventData) {

        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 11) {    //Tesoura
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            isOpen = true;
            ListaItems.Instance.caixaRevistaIsOpen = true;
            SoundController.GetInstance().PlaySound("abrindo_caixa");

            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);

            rectTransform = GetComponent<RectTransform>();
            GameObject obj = Instantiate(Item, Cena);
            ListaItems.Instance.listaItenslargados.Add(Item.GetComponent<ItemPickup>().Item);
            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x - 70, rectTransform.localPosition.y - 20, -4);
            InventoryManager.Instance.ListItems();
            Destroy(itemObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            if (!isOpen) {
                gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);
            }
        }
    }
}
