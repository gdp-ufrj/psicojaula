using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GavetaFechadura : MonoBehaviour, IDropHandler, IPointerClickHandler { 

    private bool isOpen;
    public Sprite gavetaAberta;

    private void Awake() {
        isOpen = ListaItems.Instance.gavetaIsOpen;
    }

    public void OnDrop(PointerEventData eventData) {
        GameObject itemObject = eventData.selectedObject;
        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 5 && !isOpen) {    //Cruz fria
            SoundController.GetInstance().PlaySound("abrindo_fechadura");
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            GetComponent<Image>().sprite = gavetaAberta;

            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);

            isOpen = true;
            ListaItems.Instance.gavetaIsOpen = isOpen;

            Destroy(itemObject);
        }

    }


    public void OnPointerClick(PointerEventData eventData) {
        if (isOpen) {
            GameController.GetInstance().changeScenario((int)GameController.LookDirection.OTHER, transform.gameObject.name);
        }
        else {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            dialogueTrigger.TriggerExamDialogue(true);
        }
    }
}
