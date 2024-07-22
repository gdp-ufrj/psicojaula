using UnityEngine;

public class ItemPickup : MonoBehaviour {
    public Item Item;

    void Pickup() {
        DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        InventoryManager.Instance.Add(Item, dialogueTrigger);
        SoundController.GetInstance().PlaySound(Item.nameSoundPickup);
        if (!gameObject.CompareTag("ItemDropped")) {
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerInteractionDialogue(true);
        }
        Destroy(gameObject);
    }

    void OnMouseDown() {
        if(!GameController.GetInstance().blockActionsDialogue)
            Pickup();
    }

}
