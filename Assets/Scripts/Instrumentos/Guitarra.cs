using UnityEngine;
using UnityEngine.EventSystems;

public class Guitarra : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerExamDialogue(true);
        }
        if (eventData.button == ObjDialogue.clickInteract && !ListaItems.Instance.guitarraInteragida){ 
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerInteractionDialogue(true);
            ListaItems.Instance.guitarraInteragida = true;
            TransitionController.GetInstance().LoadCutsceneMusica("guitarra");
        }
    }  
}
