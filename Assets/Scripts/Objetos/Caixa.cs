using UnityEngine;
using UnityEngine.EventSystems;

public class Caixa : MonoBehaviour, IDropHandler
{

    private RectTransform rectTransform;
    public Item revista;
    public Transform Cena;
    public GameObject Item;
    private bool isOpen = false;

    private void Awake() {
        isOpen = ListaItems.Instance.caixaIsOpen;
    }

    public void OnDrop(PointerEventData eventData)
    {

        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 11)
        {
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            isOpen = true;
            ListaItems.Instance.caixaIsOpen = true;

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);    //Dialogo de abriar a caixa

            rectTransform =  GetComponent<RectTransform>();
            
            GameObject obj =  Instantiate(Item, Cena);
            
            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x-70, rectTransform.localPosition.y-20, -4);
            InventoryManager.Instance.ListItems();

        }
        

    }

}
