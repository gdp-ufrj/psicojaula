using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Despertador : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    public GameObject Item;
    public Transform Cena;
    private bool clicked = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clicked)
        {
            DialogueTrigger dialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
            if (dialogueTrigger != null)
            {
                dialogueTrigger.TriggerExamDialogue();
            }


            rectTransform = GetComponent<RectTransform>();

            GameObject obj = Instantiate(Item, Cena);

            obj.transform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y, -4);

            clicked = true;
        }


    }
}
