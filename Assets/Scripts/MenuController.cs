using UnityEngine;

public class MenuController : MonoBehaviour {
    [SerializeField] private GameObject canvasConfigs, canvasMenu;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            canvasConfigs.SetActive(false);
            canvasMenu.SetActive(true);
        }
    }

    //Métodos para os botões do menu:
    public void StartGame() {
        TransitionController.GetInstance().LoadNextScene();
    }
    public void Configs() {
        canvasConfigs.SetActive(true);
        canvasMenu.SetActive(false);
    }
    public void ReturnToMenu() {
        canvasConfigs.SetActive(false);
        canvasMenu.SetActive(true);
    }
}
