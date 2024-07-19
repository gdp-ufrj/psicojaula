using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gaveta : MonoBehaviour, IDropHandler, IPointerClickHandler
{

    private bool isOpen;
    public Sprite gavetaAberta;


    public void OnDrop(PointerEventData eventData)
    {

        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();


        if (item.id == 5 && !isOpen){
            InventoryManager.Instance.Remove(item);
    
            GetComponent<Image>().sprite = gavetaAberta;
            
            isOpen = true;
        }

    }


    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("clicou!");
        if (isOpen) {
            GameController.GetInstance().changeScenario((int)GameController.LookDirection.OTHER, transform.gameObject.name);
        }
        //throw new System.NotImplementedException();
    }
}
