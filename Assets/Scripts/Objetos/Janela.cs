using UnityEngine;
using UnityEngine.EventSystems;

public class Janela : MonoBehaviour, IDropHandler {

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

            //if (gameObject.GetComponent<DialogueTrigger>() != null)
            //    gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);    //Dialogo de usar a revista na janela

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

            //if (gameObject.GetComponent<DialogueTrigger>() != null)
            //    gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true);    //Dialogo de usar o oculos na janela
        }

    }

}
