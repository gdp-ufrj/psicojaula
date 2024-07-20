using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    private List<ItemInventory> Items = new List<ItemInventory>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameController gameController;
    private int firstItem = 0;
    private int lastItem = 0;
    private int qtdItem = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void Add(ItemInventory itemInventory)
    {
        //Debug.Log("adicionou!");
        Items.Add(itemInventory);
        qtdItem += 1;
        if (qtdItem <= 4){
            lastItem += 1;
        }
        ListItems();
    }

    public void Remove(ItemInventory itemInventory)
    {
        if (Items.Contains(itemInventory)){
            //Debug.Log("removeu!");
            Items.Remove(itemInventory);
            qtdItem -= 1;
            if (qtdItem <= 4){
                lastItem -= 1;
            }
            ListItems();
        }
    }

    public void NextPage()
    {
        if (lastItem == qtdItem){
            return;
        }
        firstItem += 4;
        if (qtdItem < (lastItem + 4)){
            lastItem = qtdItem;
        }else{
            lastItem = lastItem + 4;
        }
        ListItems();
    }

    public void PrevPage()
    {
        if (firstItem == 0){
            return;
        }
        lastItem = firstItem;
        firstItem -= 4;
        ListItems();
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        for (int i = firstItem; i < lastItem; i++)
        {
            GameObject obj =  Instantiate(InventoryItem, ItemContent);

            var itemIcon = obj.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();

            itemIcon.sprite = Items[i].item.icon;
            obj.GetComponent<DragDrop>().AddItem(Items[i].item);
            obj.GetComponent<DragDrop>().SetGameController(gameController);

            if (Items[i].dialogueTrigger != null) {
                DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
                dialogueTrigger.SetVariables(Items[i].dialogueTrigger);
            }
        }
    }
}
