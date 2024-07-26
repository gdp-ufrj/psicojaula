using UnityEngine;
using UnityEngine.EventSystems;

public class Fogao : MonoBehaviour, IDropHandler
{

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

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);    //Di�logo de acender fog�o

        }
        if (isOnFire && item.id == 14) {     //Jornal
            SoundController.GetInstance().PlaySound("queimando_jornal");
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            rectTransform = GetComponent<RectTransform>();
            GameObject musicaDarciso = Instantiate(Item, Cena);
            musicaDarciso.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
            //InventoryManager.Instance.ListItems();

            Destroy(itemObject);

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 1);    //Di�logo de queimar objeto
        }

    }

}
