using UnityEngine;

public class BgTransition : MonoBehaviour {
    private void EndFadeInScenarioAnimation() {
        GameController.GetInstance().changeImgScenario();
    }
}
