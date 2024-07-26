using UnityEngine;
using UnityEngine.EventSystems;

public class Roupas : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {    //Bot�o esquerdo do mouse para examinar
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerExamDialogue(true);
        }
        if (eventData.button == ObjDialogue.clickInteract && !ListaItems.Instance.vestiuRoupa){    //Interagir e se não tiver vestido as roupas
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerInteractionDialogue(true);

            SoundController.GetInstance().PlaySound("roupa");
            ListaItems.Instance.vestiuRoupa = true;
        }
    }
    
}
