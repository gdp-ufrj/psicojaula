using UnityEngine;
using UnityEngine.EventSystems;

public class PortaSaida : MonoBehaviour, IDropHandler, IPointerClickHandler {
    public void OnDrop(PointerEventData eventData) {
        GameObject itemObject = eventData.selectedObject;
        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 15) {   //Chave
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);
            ListaItems.Instance.exitKeyUsed = true;
            SoundController.GetInstance().PlaySound("abrindo_fechadura");

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);
            Destroy(itemObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (ListaItems.Instance.exitKeyUsed) {
            TransitionController.GetInstance().LoadCutscene(0);   //Cutscene do final do jogo
        }
        else {
            gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);
        }
    }
}
