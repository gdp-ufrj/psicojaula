using UnityEngine;

public class MenuController : MonoBehaviour {
    [SerializeField] private GameObject canvasConfigs, canvasMenu;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            canvasConfigs.SetActive(false);
            canvasMenu.SetActive(true);
        }
    }
}
