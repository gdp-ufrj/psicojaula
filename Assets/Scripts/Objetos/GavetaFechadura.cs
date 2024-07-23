using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GavetaFechadura : MonoBehaviour, IDropHandler, IPointerClickHandler
{   //N�o ter� ObjDialogue

    private bool isOpen;
    public Sprite gavetaAberta;

    private void Awake() {
        isOpen = ListaItems.Instance.gavetaIsOpen;
    }

    public void OnDrop(PointerEventData eventData)
    {

        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();


        if (item.id == 5 && !isOpen)
        {
            SoundController.GetInstance().PlaySound("abrindo_fechadura");
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            GetComponent<Image>().sprite = gavetaAberta;

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);

            isOpen = true;
            ListaItems.Instance.gavetaIsOpen = isOpen;
        }

    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isOpen)
        {
            GameController.GetInstance().changeScenario((int)GameController.LookDirection.OTHER, transform.gameObject.name);
        }
        else {
            if (eventData.button == ObjDialogue.clickExam) { 
                DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
                if (dialogueTrigger != null)
                    dialogueTrigger.TriggerExamDialogue(true);
            }
        }
    }
}
