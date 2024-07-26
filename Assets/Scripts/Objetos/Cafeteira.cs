using UnityEngine;
using UnityEngine.EventSystems;

public class Cafeteira : MonoBehaviour, IPointerClickHandler
{

    [HideInInspector] public static PointerEventData.InputButton clickInteract = PointerEventData.InputButton.Right, clickExam = PointerEventData.InputButton.Left;
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == clickExam && !ListaItems.Instance.cafeTomado) {    //Bot�o esquerdo do mouse para examinar
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerExamDialogue(true);
        }
        if (eventData.button == clickInteract && !ListaItems.Instance.cafeTomado){    //Interagir e se não tiver tomado o cafe
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
                dialogueTrigger.TriggerInteractionDialogue(true);

            SoundController.GetInstance().PlaySound("tomando_cafe");
            ListaItems.Instance.cafeTomado = true;
        }
    }
    
}
