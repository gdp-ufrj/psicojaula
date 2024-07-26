using UnityEngine;
using UnityEngine.EventSystems;

//Esta classe estará presente nos objetos que ativarão uma mudança de cenário
public class InteractionScenario : MonoBehaviour, IPointerClickHandler{
    public void OnPointerClick(PointerEventData eventData) {
        //Debug.Log("clicou armario!");
        GameController.GetInstance().changeScenario((int)GameController.LookDirection.OTHER, transform.gameObject.name);
        //throw new System.NotImplementedException();
    }
}
