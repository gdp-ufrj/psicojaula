using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaixaRevista : MonoBehaviour
{
    public Item Item;

    void Pickup()
    {
        InventoryManager.Instance.Add(Item);
        Destroy(gameObject);
        InventoryManager.Instance.ListItems();
    }

    void OnMouseDown()
    {
        Pickup();
    }

}
