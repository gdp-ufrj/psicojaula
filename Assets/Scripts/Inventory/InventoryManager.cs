using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemInventory> Items = new List<ItemInventory>();
    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameController gameController;
    public DialogueTrigger dialogueTriggerPrefab;
    private int firstItem = 0;
    private int lastItem = 0;
    private int qtdItem = 0;

    private void Awake()
    {
        Instance = this;
        //Mantem o iventario entre as cenas
        if (ListaItems.Instance.listItems.Count >= 0) {
            Items = ListaItems.Instance.listItems;
            firstItem = ListaItems.Instance.firstItem;
            lastItem = ListaItems.Instance.lastItem;
            qtdItem = ListaItems.Instance.qtdItem ;
            ListItems();
        }
        if (ListaItems.Instance.listaItenslargados.Count > 0) {
            foreach (ItemInventory i in ListaItems.Instance.listaItenslargados){
                Add(i.item, i.dialogueTrigger);
            }
        }
        ListaItems.Instance.listaItenslargados = new List<ItemInventory>();
    }

    public void updateItems(List<ItemInventory> items, int first, int last, int qtd)
    {
        // update a lista de itens no iventario
        ListaItems.Instance.listItems = items;
        ListaItems.Instance.firstItem = first;
        ListaItems.Instance.lastItem = last;
        ListaItems.Instance.qtdItem = qtd;
    }

    public void Add(Item item, DialogueTrigger dialogueTrigger)
    {
        ItemInventory itemInventory;
        if (dialogueTrigger != null)
        {
            DialogueTrigger newDialogueTrigger = Instantiate(dialogueTriggerPrefab);
            newDialogueTrigger.SetVariables(dialogueTrigger);
            itemInventory = new ItemInventory(item, newDialogueTrigger);
        }
        else
            itemInventory = new ItemInventory(item, null);

        Items.Add(itemInventory);
        qtdItem += 1;
        if (qtdItem <= 4)
        {
            lastItem += 1;
        }
        else if (lastItem % 4 != 0)
        {
            lastItem += 1;
        }
        ListItems();
        updateItems(Items, firstItem, lastItem, qtdItem);
    }

    public void Remove(ItemInventory itemInventory)
    {
        if (Items.Contains(itemInventory))
        {
            //Destroy(Items[Items.IndexOf(itemInventory)].dialogueTrigger.gameObject);
            Items.Remove(itemInventory);
            qtdItem -= 1;
            if (qtdItem <= 4)
            {
                lastItem -= 1;
            }
            else if (lastItem % 4 != 0)
            {
                lastItem -= 1;
            }
            ListItems();
            updateItems(Items, firstItem, lastItem, qtdItem);
        }
    }

    public void NextPage()
    {
        if (lastItem == qtdItem)
        {
            return;
        }
        firstItem += 4;
        if (qtdItem < (lastItem + 4))
        {
            lastItem = qtdItem;
        }
        else
        {
            lastItem = lastItem + 4;
        }
        ListItems();
    }

    public void PrevPage()
    {
        if (firstItem == 0)
        {
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
            GameObject obj = Instantiate(InventoryItem, ItemContent);

            var itemIcon = obj.transform.Find("Image").GetComponent<UnityEngine.UI.Image>();

            itemIcon.sprite = Items[i].item.icon;
            obj.GetComponent<DragDrop>().AddItem(Items[i].item);
            obj.GetComponent<DragDrop>().SetGameController(gameController);

            if (Items[i].dialogueTrigger != null)
            {
                DialogueTrigger dialogueTrigger = obj.GetComponent<DialogueTrigger>();
                dialogueTrigger.SetVariables(Items[i].dialogueTrigger);
            }
        }
    }
}
