using UnityEngine;
using UnityEngine.EventSystems;

public class PosterBanda : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (GameController.GetInstance().gamePhase == 1){
                dialogueTrigger.TriggerExamDialogue(true, 0);
            }
            else {
                dialogueTrigger.TriggerExamDialogue(true, 1);
            }
        }
    }
}