using UnityEngine;
using UnityEngine.EventSystems;

public class Guitarra : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (!ListaItems.Instance.guitarraInteragida) {
                dialogueTrigger.TriggerExamDialogue(true, 0);
            }
            else {
                dialogueTrigger.TriggerExamDialogue(true, 1);
            }
        }
        if (eventData.button == ObjDialogue.clickInteract && !ListaItems.Instance.guitarraInteragida){ 
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0, true);
            ListaItems.Instance.guitarraInteragida = true;
            TransitionController.GetInstance().LoadCutsceneMusica("guitarra");
        }
    }  
}
