using UnityEngine;
using UnityEngine.EventSystems;

public class TV : MonoBehaviour, IPointerClickHandler {
    private RectTransform rectTransform;
    public Transform Cena;
    public GameObject Item;
    public Item item;

    public void OnPointerClick(PointerEventData eventData){
        if (eventData.button == ObjDialogue.clickInteract) {
            if (GameController.GetInstance().gamePhase == 3) {
                SoundController.GetInstance().PlaySound("ligando_TV");
                GameController.GetInstance().changeTV();
            }
            else {
                if (ListaItems.Instance.oculosUsado && !ListaItems.Instance.musicaColetadaTv) {
                    rectTransform = GetComponent<RectTransform>();
                    GameObject obj = Instantiate(Item, Cena);
                    obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
                    ListaItems.Instance.musicaColetadaTv = true;

                    SoundController.GetInstance().PlaySound("ligando_TV");
                    gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);
                }
            }
        }
        else if (eventData.button == ObjDialogue.clickExam) {
            if(GameController.GetInstance().gamePhase == 3) {
                gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true, 2);
            }
            else {
                if (ListaItems.Instance.oculosUsado)
                    gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true, 1);
                else
                    gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true, 0);
            }
        }
    }
}
