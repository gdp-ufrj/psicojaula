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
                //Debug.Log(itemObject.GetComponents<DialogueTrigger>().Length);
                DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
                ItemInventory itemInventory = new ItemInventory(dropItem, dialogueTrigger);
                ItemInventory itemInventory2 = new ItemInventory(Item, dialogueTrigger);
                ListaItems.Instance.listaItenslargados.Remove(itemInventory2);
                InventoryManager.Instance.Remove(itemInventory);

                //GameObject obj = Instantiate(ItemObject, itemObject.transform.parent);
                GameObject obj = Instantiate(dropItem.newItemPrefab, itemObject.transform.parent);
                obj.GetComponent<RectTransform>().localPosition = itemObject.GetComponent<RectTransform>().localPosition;

                //var itemIcon = obj.GetComponent<UnityEngine.UI.Image>(); 
                
                //itemIcon.sprite = dropItem.newItem.icon;
                //obj.GetComponent<ItemPickup>().Item = dropItem.newItem;
                //obj.GetComponent<ItemController>().Item = dropItem.newItem;

                //obj.GetComponent<DialogueTrigger>().SetVariables(dialogueTrigger);
                
                Destroy(itemObject); 
                Destroy(gameObject); 
            }
        }
    }

}
