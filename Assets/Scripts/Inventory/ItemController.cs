using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour, IDropHandler 
{
    public Item Item;
    public GameObject ItemObject;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject itemObject = eventData.selectedObject;
        Debug.Log("OIIIIIIIIIIIIIIIIIIII");
        var dropItem = itemObject.GetComponent<DragDrop>().GetItem();

        if (Item.id == 0 || Item.id == 1) {
            if (dropItem.id ==  0 || dropItem.id == 1) {
                
                InventoryManager.Instance.Remove(dropItem);    

                GameObject obj = Instantiate(ItemObject, itemObject.transform.parent);

                var itemIcon = obj.GetComponent<UnityEngine.UI.Image>(); 
                obj.GetComponent<RectTransform>().localPosition = itemObject.GetComponent<RectTransform>().localPosition;
                
                itemIcon.sprite = dropItem.newItem.icon;
                obj.GetComponent<ItemPickup>().Item = dropItem.newItem;
                obj.GetComponent<ItemController>().Item = dropItem.newItem;
                
                Destroy(itemObject); 
                Destroy(GetComponent<RectTransform>().gameObject); 
            }
        }
    }

}
