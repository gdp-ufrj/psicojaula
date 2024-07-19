using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Caixa : MonoBehaviour, IDropHandler
{

    private RectTransform rectTransform;
    public Transform Cena;
    public GameObject Item;
    private bool Used;


    public void OnDrop(PointerEventData eventData)
    {

        GameObject itemObject = eventData.selectedObject;

        var item = itemObject.GetComponent<DragDrop>().GetItem();


        if (item.id == 11 && !Used){
            InventoryManager.Instance.Remove(item);
            
            rectTransform =  GetComponent<RectTransform>();
            
            GameObject obj =  Instantiate(Item, Cena);
            
            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x + 90, rectTransform.localPosition.y - 30, -4);
            Debug.Log(obj.transform.localPosition);
            InventoryManager.Instance.ListItems();
            Used = true;
        }

    }

}
