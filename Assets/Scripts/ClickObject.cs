using UnityEngine;
using UnityEngine.EventSystems;

//Esta classe estará presente nos objetos de cenário
public class ClickObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {
    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("clicou!");
        GameController.GetInstance().changeScenario((int)GameController.LookDirection.OTHER, transform.gameObject.name);
        //throw new System.NotImplementedException();
    }

    public void OnPointerDown(PointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData) {
        //throw new System.NotImplementedException();
    }

    public void OnPointerUp(PointerEventData eventData) {
        //throw new System.NotImplementedException();
    }
}
