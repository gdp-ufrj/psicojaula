using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IDropHandler 
{
    public Item Item;
    public GameObject ItemObject;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject itemObject = eventData.selectedObject;
        var dropItem = itemObject.GetComponent<DragDrop>().GetItem();

        if (Item.id == 0 || Item.id == 1) {    //Combinando jornais
            if (dropItem.id ==  0 || dropItem.id == 1) {
                DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
                ItemInventory itemInventory = new ItemInventory(dropItem, dialogueTrigger);
                ItemInventory itemInventory2 = new ItemInventory(Item, dialogueTrigger);
                ListaItems.Instance.listaItenslargados.Remove(Item);
                InventoryManager.Instance.Remove(itemInventory);

                GameObject obj = Instantiate(dropItem.newItemPrefab, itemObject.transform.parent);
                ListaItems.Instance.listaItenslargados.Add(dropItem.newItemPrefab.GetComponent<ItemPickup>().Item);
                obj.GetComponent<RectTransform>().localPosition = itemObject.GetComponent<RectTransform>().localPosition;
                Destroy(itemObject); 
                Destroy(gameObject); 
            }
        }
    }

}
