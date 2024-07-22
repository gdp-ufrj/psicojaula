using UnityEngine;
using UnityEngine.EventSystems;

public class ObjDialogue : MonoBehaviour, IPointerClickHandler {

    [HideInInspector] public static PointerEventData.InputButton clickInteract = PointerEventData.InputButton.Right, clickExam = PointerEventData.InputButton.Left;

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == clickExam) {    //Botão esquerdo do mouse para examinar, e direito para interagir
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerExamDialogue(true);
        }
    }
}
