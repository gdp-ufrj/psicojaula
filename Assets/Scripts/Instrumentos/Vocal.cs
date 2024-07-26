using UnityEngine;
using UnityEngine.EventSystems;

public class Vocal : MonoBehaviour, IDropHandler, IPointerClickHandler {
    public void OnDrop(PointerEventData eventData) {
        GameObject itemObject = eventData.selectedObject;
        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 18) {   //Fernanda
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);
            ListaItems.Instance.musicaVocal = true;
            Destroy(itemObject);
            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);
            TransitionController.GetInstance().LoadCutsceneMusica("vocal");
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerExamDialogue(true);
        }
    }
}