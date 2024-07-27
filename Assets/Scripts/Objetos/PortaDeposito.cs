using UnityEngine;
using UnityEngine.EventSystems;

public class PortaDeposito : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        if (GameController.GetInstance().gamePhase == 1) {
            if (!ListaItems.Instance.cafeTomado || !ListaItems.Instance.vestiuRoupa || !ListaItems.Instance.remedioTomado)
                gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);
            else
                TransitionController.GetInstance().LoadDeposito();
        }
        else
            TransitionController.GetInstance().LoadDeposito();
    }
}
