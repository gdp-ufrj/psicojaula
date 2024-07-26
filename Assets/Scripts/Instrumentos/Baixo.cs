using UnityEngine;
using UnityEngine.EventSystems;

public class Baixo : MonoBehaviour, IDropHandler {
    public void OnDrop(PointerEventData eventData) {

        GameObject itemObject = eventData.selectedObject;
        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 16) {   //Darciso
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);
            ListaItems.Instance.musicaBaixo = true;
            Destroy(itemObject);
            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);
            TransitionController.GetInstance().LoadCutsceneMusica("baixo");
        }
    }
}