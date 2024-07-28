using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Frigobar : MonoBehaviour, IDropHandler, IPointerClickHandler {

    private RectTransform rectTransform;
    public Item cruz_fria;
    public Transform Cena;
    public GameObject Item;
    public GameObject Presunto;
    public Sprite frigobar_aberto;
    public Sprite frigobar_fechado;

    private bool isOpen = false; 

    private void Awake() {
        int x = 40;
        int y = 15;

        if (ListaItems.Instance.frigobarIsOpen) {
            GetComponent<Image>().sprite = frigobar_aberto;
            rectTransform = GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x + x, rectTransform.localPosition.y + y, rectTransform.localPosition.z);
            rectTransform.sizeDelta = new Vector2((float)161.76, 175);
            if (Presunto != null) {
                Presunto.SetActive(true);
            }
            isOpen = true;
        }
    }    
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.button == ObjDialogue.clickInteract) {    //Com o bot�o direito, estaremos interagindo com o objeto do cen�rio
            int x = 40;
            int y = 15;

            if (isOpen) {
                SoundController.GetInstance().PlaySound("fechando_frigobar");
                GetComponent<Image>().sprite = frigobar_fechado;
                rectTransform = GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x - x, rectTransform.localPosition.y - y, rectTransform.localPosition.z);
                rectTransform.sizeDelta = new Vector2(100, 150);
                isOpen = false;
                ListaItems.Instance.frigobarIsOpen = isOpen;
                if (Presunto != null) {
                    Presunto.SetActive(false);
                }
            }
            else {
                SoundController.GetInstance().PlaySound("abrindo_frigobar");
                GetComponent<Image>().sprite = frigobar_aberto;
                rectTransform = GetComponent<RectTransform>();
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x + x, rectTransform.localPosition.y + y, rectTransform.localPosition.z);
                rectTransform.sizeDelta = new Vector2((float)161.76, 175);
                isOpen = true;
                ListaItems.Instance.frigobarIsOpen = isOpen;
                if (Presunto != null) {
                    Presunto.SetActive(true);
                }
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {

        GameObject itemObject = eventData.selectedObject;

        Item item = itemObject.GetComponent<DragDrop>().GetItem();

        if (item.id == 4 && isOpen){    //Cruz quente
            DialogueTrigger dialogueTrigger = itemObject.GetComponent<DialogueTrigger>();
            ItemInventory itemInventory = new ItemInventory(item, dialogueTrigger);
            InventoryManager.Instance.Remove(itemInventory);
            
            rectTransform =  GetComponent<RectTransform>();
            GameObject obj =  Instantiate(Item, Cena);
            ListaItems.Instance.listaItenslargados.Add(Item.GetComponent<ItemPickup>().Item);

            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);
            InventoryManager.Instance.ListItems();

            SoundController.GetInstance().PlaySound("colocando_cruz");
            gameObject.GetComponent<DialogueTrigger>().TriggerInteractionDialogue(true, 0);
            
            Destroy(itemObject);
        }

    }

}
