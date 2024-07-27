using UnityEngine;
using UnityEngine.EventSystems;

public class Painting : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        if (GameController.GetInstance().gamePhase == 1) {
            dialogueTrigger.TriggerExamDialogue(true, 0);
        }
        else if (GameController.GetInstance().gamePhase == 2) {
            dialogueTrigger.TriggerExamDialogue(true, 1);
        }
        else if (GameController.GetInstance().gamePhase == 3) {
            dialogueTrigger.TriggerExamDialogue(true, 2);
        }
    }
}