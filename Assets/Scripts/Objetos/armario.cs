using UnityEngine;
using UnityEngine.EventSystems;

public class armario : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("clicou armario!");
        //GameController.GetInstance().changeScenario((int)GameController.LookDirection.OTHER, transform.gameObject.name);
        //throw new System.NotImplementedException();
    }
}
