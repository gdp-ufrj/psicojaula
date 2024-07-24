using UnityEngine;
using UnityEngine.EventSystems;

public class Luminaria : MonoBehaviour, IDropHandler
{

    private RectTransform rectTransform;
    public Item musica;
    public Transform Cena;
    public GameObject Item;
    private bool musicaColetada = false;

    private void Awake() {
        musicaColetada = ListaItems.Instance.musicaColetadaLuminaria;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 6)
        {
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);

            musicaColetada = true;
            ListaItems.Instance.musicaColetadaLuminaria = true;

            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);    //Dialogo de 

            rectTransform =  GetComponent<RectTransform>();
            
            GameObject obj =  Instantiate(Item, Cena);
            
            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
            InventoryManager.Instance.ListItems();

            Destroy(itemObject);

        }
        

    }

}
