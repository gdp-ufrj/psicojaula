using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Frigobar : MonoBehaviour, IDropHandler
{

    private RectTransform rectTransform;
    private Image image;
    public Item cruz_fria;
    public Transform Cena;
    public GameObject Item;
    public Sprite frigobar_aberto;
    public Sprite frigobar_fechado;

    private bool isOpen = false; 

    public void OnMouseDown(){
        int x = 40;
        int y = 15;


        if(isOpen){
            GetComponent<Image>().sprite = frigobar_fechado;
            rectTransform =  GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - x, rectTransform.localPosition.y - y, rectTransform.localPosition.z);
            rectTransform.sizeDelta = new Vector2(100, 150);
            isOpen = false;
        }else{
            GetComponent<Image>().sprite = frigobar_aberto;
            rectTransform =  GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x + x, rectTransform.localPosition.y + y, rectTransform.localPosition.z);
            rectTransform.sizeDelta = new Vector2((float)161.76, 175);
            isOpen = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        GameObject itemObject = eventData.selectedObject;

        Item item = itemObject.GetComponent<DragDrop>().GetItem();

        //Debug.Log(item.id);
        if (item.id == 4 && isOpen){    //Puzzle cruz
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);
            //InventoryManager.Instance.Add(cruz_fria);
            
            rectTransform =  GetComponent<RectTransform>();
            
            GameObject obj =  Instantiate(Item, Cena);
            
            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
            Debug.Log(obj.transform.localPosition);
            InventoryManager.Instance.ListItems();
        }

    }

}
