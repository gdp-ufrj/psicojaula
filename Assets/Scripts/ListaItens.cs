using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaItems : MonoBehaviour
{
    public static ListaItems Instance;
    public List<ItemInventory> listItems = new List<ItemInventory>();
    public int firstItem = 0;
    public int lastItem = 0;
    public int qtdItem = 0;
    private void Awake()
    {
        
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    
    
}
