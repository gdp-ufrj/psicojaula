using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;

public class Roupas : MonoBehaviour, IPointerClickHandler
{

    [HideInInspector] public static PointerEventData.InputButton clickInteract = PointerEventData.InputButton.Right, clickExam = PointerEventData.InputButton.Left;
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == clickExam) {    //Bot�o esquerdo do mouse para examinar
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerExamDialogue(true);
        }
        if (eventData.button == clickInteract && !ListaItems.Instance.vestiuRoupa){    //Interagir e se não tiver vestido as roupas
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerInteractionDialogue(true);
        
            ListaItems.Instance.vestiuRoupa = true;
        }
    }
    
}
