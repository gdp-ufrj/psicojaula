using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionController : MonoBehaviour {
    private static TransitionController instance;

    public GameObject bgTransitions;
    private Animator animTransitionScenes;
    [SerializeField] private float transistionTimeScenes, transitionTimeScenarios, transitionTimeCutscenes;

    private int cutscene = -1, activeCutscene = -1;  //cutscene guarda qual das cutscenes está rodando (nenhuma se for -1), e activeCutscene guarda qual imagem da cutscene está aparecendo (nenhuma se for -1)
    private bool canPassCutscene = false;
    [SerializeField] private GameObject cutscenes;

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
        //Quando uma cena carregar, a tela vai ficar preta e o fadeOut s� vai acontecer quando tudo j� estiver pronto
        Color colorBgTransition = Color.black;
        colorBgTransition.a = 1;
        bgTransitions.GetComponent<Image>().color = colorBgTransition;
        animTransitionScenes = bgTransitions.GetComponent<Animator>();
    }

    private void Update() {
        if (canPassCutscene) {
            if(Input.GetKeyDown(KeyCode.Mouse0) ||  Input.GetKeyDown(KeyCode.Space)) {
                StartCoroutine(NextCutscene());
            }
        }
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
    public void LoadQuarto() {
        StartCoroutine(LoadSceneOrScenario(SceneManager.GetActiveScene().buildIndex - 1));   //Carregando o Quarto
    }
    public void LoadDeposito() {
        StartCoroutine(LoadSceneOrScenario(SceneManager.GetActiveScene().buildIndex + 1));   //Carregando o Deposito
    }
    public void LoadNextFase() {
        StartCoroutine(LoadSceneOrScenario(SceneManager.GetActiveScene().buildIndex + 2));   //Carregando a proxima fase
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


    public void LoadCutscene(int cutsceneIndex) {
        cutscene = cutsceneIndex;
        cutscenes.SetActive(true);
        cutscenes.transform.GetChild(cutsceneIndex).gameObject.SetActive(true);
        StartCoroutine(NextCutscene());
    }

    private IEnumerator NextCutscene() {
        canPassCutscene = false;
        //Debug.Log("opacity: " + bgTransitions.GetComponent<Image>().color.a);
        if(activeCutscene == cutscenes.transform.GetChild(cutscene).transform.childCount - 1) {
            bgTransitions.GetComponent<Image>().raycastTarget = true;
            animTransitionScenes.Play("fadeInCutscene");
            yield return new WaitForSeconds(transitionTimeCutscenes);
            cutscenes.transform.GetChild(cutscene).transform.GetChild(activeCutscene).gameObject.SetActive(false);
            cutscenes.transform.GetChild(cutscene).gameObject.SetActive(false);
            cutscenes.gameObject.SetActive(false);
            activeCutscene = -1;
            cutscene = -1;
            FadeOutScene();
        }
        else {
            if (bgTransitions.GetComponent<Image>().color.a != 1) {
                bgTransitions.GetComponent<Image>().raycastTarget = true;
                animTransitionScenes.Play("fadeInCutscene");
                yield return new WaitForSeconds(transitionTimeCutscenes);
            }
            if (activeCutscene != -1)
                cutscenes.transform.GetChild(cutscene).transform.GetChild(activeCutscene).gameObject.SetActive(false);
            activeCutscene++;
            cutscenes.transform.GetChild(cutscene).transform.GetChild(activeCutscene).gameObject.SetActive(true);
            animTransitionScenes.Play("fadeOutCutscene");
            canPassCutscene = true;
        }
    }


    private IEnumerator LoadSceneOrScenario(int sceneIndex = -1) {
        if (sceneIndex != -1) {   //Se for a transi��o entre cenas
            SoundController.GetInstance().PauseCurrentTrack();
            bgTransitions.GetComponent<Image>().raycastTarget = true;
            animTransitionScenes.Play("fadeInScene");
            yield return new WaitForSeconds(transistionTimeScenes);
            SceneManager.LoadScene(sceneIndex);
        }
        else {    //Se for transi��o entre cen�rios
            bgTransitions.GetComponent<Image>().raycastTarget = true;
            animTransitionScenes.Play("fadeInScenario");
            yield return new WaitForSeconds(transitionTimeScenarios);
            bgTransitions.GetComponent<Image>().raycastTarget = false;
            animTransitionScenes.Play("fadeOutScenario");
        }
    }
}
