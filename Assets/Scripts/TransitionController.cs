using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour {
    private static TransitionController instance;

    public GameObject bgTransitions;
    private Animator animTransitionScenes;
    [SerializeField] private float transistionTimeScenes, transitionTimeScenarios;

    public static TransitionController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start() {
        //Quando uma cena carregar, a tela vai ficar preta e o fadeOut só vai acontecer quando tudo já estiver pronto
        Color colorBgTransition = Color.black;
        colorBgTransition.a = 1;
        bgTransitions.GetComponent<Image>().color = colorBgTransition;
        animTransitionScenes = bgTransitions.GetComponent<Animator>();
    }

    public void FadeOutScene() {
        animTransitionScenes.Play("fadeOutScene");
        bgTransitions.GetComponent<Image>().raycastTarget = false;
    }

    public void LoadNextScene() {
        if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) {
            StartCoroutine(LoadSceneOrScenario(0));   //Carregando a primeira cena novamente (menu)
        }
        else
            StartCoroutine(LoadSceneOrScenario(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadScenario() {
        StartCoroutine(LoadSceneOrScenario());
    }


    public void LoadMainScene() {
        StartCoroutine(LoadSceneOrScenario(1));   //Carregando a cena principal
    }
    public void LoadMenu() {
        StartCoroutine(LoadSceneOrScenario(0));   //Carregando o menu
    }

    private IEnumerator LoadSceneOrScenario(int sceneIndex=-1) {
        if (sceneIndex != -1) {   //Se for a transição entre cenas
            bgTransitions.GetComponent<Image>().raycastTarget = true;
            animTransitionScenes.Play("fadeInScene");
            yield return new WaitForSeconds(transistionTimeScenes);
            SceneManager.LoadScene(sceneIndex);
        }
        else {    //Se for transição entre cenários
            bgTransitions.GetComponent<Image>().raycastTarget = true;
            animTransitionScenes.Play("fadeInScenario");
            yield return new WaitForSeconds(transitionTimeScenarios);
            bgTransitions.GetComponent<Image>().raycastTarget = false;
            animTransitionScenes.Play("fadeOutScenario");
        }
    }
}
