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

    public bool musicaBaixo = false;
    public bool musicaBateria = false;
    public bool musicaVocal = false;
    public bool musicaTeclado = false;
    public bool exitKeyUsed = false;
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

    public void resetVariables() {
        foreach (Transform child in gameObject.transform) {
            GameObject.Destroy(child.gameObject);
        }
        ItensColetados = new List<Item>();
        listItems = new List<ItemInventory>();
        listaItenslargados = new List<ItemInventory>();
        firstItem = 0;
        lastItem = 0;
        qtdItem = 0;
        frigobarIsOpen = false;
        despertadorIsClicked = false;
        mesaTVIsClicked = false;
        isOnFire = false;
        gavetaIsOpen = false;
        caixaRevistaIsOpen = false;
        musicaColetadaJanela = false;
        musicaColetadaLuminaria = false;
        musicaColetadaTv = false;
        oculosUsado = false;
        cafeTomado = false;
        vestiuRoupa = false;
        remedioTomado = false;
        guitarraInteragida = false;
        comeuPresunto = false;

        musicaBaixo = false;
        musicaBateria = false;
        musicaVocal = false;
        musicaTeclado = false;
        exitKeyUsed = false;
    }
}
