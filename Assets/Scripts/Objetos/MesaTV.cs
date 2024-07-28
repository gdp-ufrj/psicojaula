using UnityEngine;
using UnityEngine.EventSystems;

public class MesaTV : MonoBehaviour, IPointerClickHandler {
    private RectTransform rectTransform;
    public GameObject Item;
    public Transform Cena;
    private bool clicked = false;

    private void Awake() {
        clicked = ListaItems.Instance.mesaTVIsClicked;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickInteract) {
            if (!clicked) {
                rectTransform = GetComponent<RectTransform>();
                GameObject obj = Instantiate(Item, Cena);
                ListaItems.Instance.listaItenslargados.Add(Item.GetComponent<ItemPickup>().Item);
                obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
                clicked = true;

                ListaItems.Instance.mesaTVIsClicked = clicked;
                SoundController.GetInstance().PlaySound("abrindo_gaveta");
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);
            }
        }

        else if (eventData.button == ObjDialogue.clickExam) {
            if (!clicked)
                gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);
        }
    }
}
