using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TV : MonoBehaviour, IPointerClickHandler
{


    private RectTransform rectTransform;
    public Transform Cena;
    public GameObject Item;
    public Item item;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (ListaItems.Instance.oculosUsado && !ListaItems.Instance.musicaColetadaTv){
            DialogueTrigger dialogueTrigger = Item.GetComponent<DialogueTrigger>();
            
            InventoryManager.Instance.Add(item, dialogueTrigger);
            
            if (gameObject.GetComponent<DialogueTrigger>() != null)
                gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0); // dialogo de anotar a musica do lucas

            
            InventoryManager.Instance.ListItems();
            ListaItems.Instance.musicaColetadaTv = true;
        }
    }
}
