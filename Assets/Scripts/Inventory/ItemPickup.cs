using UnityEngine;

public class ItemPickup : MonoBehaviour {
    public Item Item;

    void Pickup() {
        DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        ItemInventory itemInventory = new ItemInventory(Item, dialogueTrigger);
        InventoryManager.Instance.Add(itemInventory);
        //InventoryManager.Instance.ListItems();
        SoundController.GetInstance().PlaySound(Item.nameSoundPickup);
        if (!gameObject.CompareTag("ItemDropped")) {
            if (dialogueTrigger != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue();
        }
        Destroy(gameObject);
    }

    void OnMouseDown() {
        Pickup();
    }

}
