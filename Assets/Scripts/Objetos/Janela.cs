using UnityEngine;
using UnityEngine.EventSystems;

public class Janela : MonoBehaviour, IDropHandler, IPointerClickHandler {

    private RectTransform rectTransform;
    public Item musica;
    public Transform Cena;
    public GameObject Item;
    private bool musicaColetada = false;
    private bool oculosUsado = false;

    private void Awake() {
        musicaColetada = ListaItems.Instance.musicaColetadaJanela;
        oculosUsado = ListaItems.Instance.oculosUsado;
    }

    public void OnDrop(PointerEventData eventData) {
        GameObject itemObject = eventData.selectedObject;
        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 13  && !musicaColetada) {
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            musicaColetada = true;
            ListaItems.Instance.musicaColetadaJanela = true;

            Destroy(itemObject);

            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);    //Revista na janela

            rectTransform =  GetComponent<RectTransform>();
            GameObject musicaFernanda =  Instantiate(Item, Cena);
            musicaFernanda.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y-60, -4);
            InventoryManager.Instance.ListItems();
        }
        
        if (item.id == 9 && !oculosUsado) {
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            oculosUsado = true;
            ListaItems.Instance.oculosUsado = true;

            Destroy(itemObject);

            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 1);    //Óculos na janela
        }

    }

    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickExam) {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (GameController.GetInstance().gamePhase == 1) {
                dialogueTrigger.TriggerExamDialogue(true, 0);
                Debug.Log("kdkoe");
            }
            else if (GameController.GetInstance().gamePhase == 2)
                dialogueTrigger.TriggerExamDialogue(true, 1);
            else if (GameController.GetInstance().gamePhase == 3)
                dialogueTrigger.TriggerExamDialogue(true, 2);
        }
    }

}
