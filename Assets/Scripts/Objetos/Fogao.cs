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

    public void OnDrop(PointerEventData eventData)
    {

        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();


        Debug.Log(item.id);
        if (item.id == 3)
        {
            SoundController.GetInstance().PlaySound("acendendo_fogao");
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            isOnFire = true;
            ListaItems.Instance.isOnFire = true;

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);    //Di�logo de acender fog�o

        }
        if (isOnFire && item.id == 14)
        {
            SoundController.GetInstance().PlaySound("queimando_jornal");
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            rectTransform = GetComponent<RectTransform>();

            GameObject obj = Instantiate(Item, Cena);

            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
            Debug.Log(obj.transform.localPosition);
            InventoryManager.Instance.ListItems();
            Destroy(itemObject);

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 1);    //Di�logo de queimar objeto
        }

    }

}
