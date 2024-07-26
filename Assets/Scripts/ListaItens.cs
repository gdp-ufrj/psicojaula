using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListaItems : MonoBehaviour
{
    public static ListaItems Instance;
    public List<ItemInventory> listItems = new List<ItemInventory>();
    public List<ItemInventory> listaItenslargados = new List<ItemInventory>();
    public GameObject quartoScenarios, depositoScenarios;
    public List<Item> ItensColetados = new List<Item>();

    public int firstItem = 0;
    public int lastItem = 0;
    public int qtdItem = 0;
    public bool frigobarIsOpen;
    public bool despertadorIsClicked = false;
    public bool mesaTVIsClicked = false;
    public bool isOnFire = false;
    public bool gavetaIsOpen = false;
    public bool caixaRevistaIsOpen = false;
    public bool musicaColetadaJanela = false;
    public bool musicaColetadaLuminaria = false;
    public bool musicaColetadaTv = false;
    public bool oculosUsado = false;
    public bool cafeTomado = false;
    public bool vestiuRoupa = false;
    public bool remedioTomado = false;
    public bool guitarraInteragida = false;
    public bool comeuPresunto = false;
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
