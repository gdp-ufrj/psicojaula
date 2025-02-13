using UnityEngine;
using UnityEngine.EventSystems;

public class Fogao : MonoBehaviour, IDropHandler, IPointerClickHandler {

    private RectTransform rectTransform;
    public Item musica;
    public Transform Cena;
    public GameObject Item;
    private bool isOnFire = false;

    private void Awake() {
        isOnFire = ListaItems.Instance.isOnFire;
    }

    public void OnDrop(PointerEventData eventData) {

        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 3) {    //Fósforo
            SoundController.GetInstance().PlaySound("acendendo_fogao");
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            isOnFire = true;
            ListaItems.Instance.isOnFire = true;

            Destroy(itemObject);

            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);    //Di�logo de acender fog�o

        }
        if (isOnFire && item.id == 14) {     //Jornal
            SoundController.GetInstance().PlaySound("queimando_jornal");
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            rectTransform = GetComponent<RectTransform>();
            GameObject musicaDarciso = Instantiate(Item, Cena);
            ListaItems.Instance.listaItenslargados.Add(Item.GetComponent<ItemPickup>().Item);
            musicaDarciso.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);

            Destroy(itemObject);

            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 1);    //Di�logo de queimar objeto
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (!isOnFire) {
                dialogueTrigger.TriggerExamDialogue(true, 0);
            }
            else {
                dialogueTrigger.TriggerExamDialogue(true, 1);
            }
        }
    }

}
