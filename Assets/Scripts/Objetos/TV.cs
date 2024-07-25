using UnityEngine;
using UnityEngine.EventSystems;

public class TV : MonoBehaviour, IPointerClickHandler
{
    public Transform Cena;
    public GameObject Item;
    public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == ObjDialogue.clickInteract) {
            if (ListaItems.Instance.oculosUsado) {
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 1); // dialogo de anotar a musica do lucas
                if (!ListaItems.Instance.musicaColetadaTv) {
                    DialogueTrigger dialogueTrigger = Item.GetComponent<DialogueTrigger>();

                    InventoryManager.Instance.Add(item, dialogueTrigger);

                    InventoryManager.Instance.ListItems();
                    ListaItems.Instance.musicaColetadaTv = true;
                }
            }
            else
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);
        }
        else if (eventData.button == ObjDialogue.clickExam) {
            if (ListaItems.Instance.oculosUsado)
                gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true, 1);
            else
                gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true, 0);
        }
    }
}
