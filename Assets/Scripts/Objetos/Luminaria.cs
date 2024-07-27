using UnityEngine;
using UnityEngine.EventSystems;

public class Luminaria : MonoBehaviour, IDropHandler, IPointerClickHandler {

    private RectTransform rectTransform;
    public Item musica;
    public Transform Cena;
    public GameObject Item;

    public void OnDrop(PointerEventData eventData) {
        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 6) {    //Certidão
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            ListaItems.Instance.musicaColetadaLuminaria = true;
            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);

            SoundController.GetInstance().PlaySound("certidao_luminaria");

            rectTransform =  GetComponent<RectTransform>();
            GameObject obj =  Instantiate(Item, Cena);
            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
            InventoryManager.Instance.ListItems();
            Destroy(itemObject);

        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            dialogueTrigger.TriggerExamDialogue(true, 0);
        }
    }
}
