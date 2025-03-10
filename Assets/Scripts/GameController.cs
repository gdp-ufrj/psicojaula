using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    private static GameController instance;

    public enum LookDirection {
        LEFT,
        RIGHT,
        UP,
        BACK,
        OTHER
    }

    private ScenarioDictionary scenarios;   //Todos os cen�rios do jogo (todos come�ar�o desativados)  (n�o � poss�vel serializar)
    private ScenarioDictionaryItem currentScenario, newScenario;

    [SerializeField] private GameObject[] scenariosListInOrder;    //Essa ser� uma lista serializada, na qual poderemos colocar os GameObjects de cen�rio

    [SerializeField] private GameObject canvasScenarios, btnLeft, btnRight, btnUp, btnBack, canvasCutsceneMusica;
    [SerializeField] private GameObject canvasPause, canvasMenu, canvasConfigs;  //Diferentes interfaces do jogo
    [SerializeField] private GameObject TV_estatica, TV_piano, TV_medo1, TV_medo2, TV_medo3;
    [SerializeField] private Slider OSTVolumeSlider, SFXVolumeSlider;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraOffset;

    public GameObject exitKey;
    private int idActiveScenario;
    public int gamePhase;
    private bool isChangingScenario = false, gamePaused=false, isInMainScenario, isInQuarto=false;
    public bool blockActionsDialogue = false;
    private float originalCameraSize;


    public static GameController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public GameObject GetActiveScene() {
        return currentScenario.ScenarioObject;
    }

    private void Start() {
        if(TV_estatica != null && TV_piano != null) {
            if (ListaItems.Instance.oculosUsado) {
                TV_estatica.SetActive(false);
                TV_piano.SetActive(true);
            }
            else {
                TV_estatica.SetActive(true);
                TV_piano.SetActive(false);
            }
        }

        if(exitKey != null) {
            if (ListaItems.Instance.musicaBaixo && ListaItems.Instance.musicaBateria && !ListaItems.Instance.pegouchaveFinal)
                exitKey.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name.ToUpper().Contains("_1"))
            gamePhase = 1;
        else if (SceneManager.GetActiveScene().name.ToUpper().Contains("_2"))
            gamePhase = 2;
        else if (SceneManager.GetActiveScene().name.ToUpper().Contains("_3"))
            gamePhase = 3;
        else
            gamePhase = -1;

        bool canPlaySceneMusic = true;
        if (SceneManager.GetActiveScene().name.ToUpper().Contains("DEPOSITO")) {
            isInQuarto = false;
            if (ListaItems.Instance.depositoScenarios != null){
                canvasScenarios = ListaItems.Instance.depositoScenarios;
            }
        }
        else if (SceneManager.GetActiveScene().name.ToUpper().Contains("QUARTO")) {
            isInQuarto = true;
            if (ListaItems.Instance.quartoScenarios != null){
                canvasScenarios = ListaItems.Instance.quartoScenarios;
            }
            if(gamePhase == 1) {
                if (ListaItems.Instance.firstTimeInPhase1) {
                    gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);
                    canPlaySceneMusic = false;
                }
            }
            else if (gamePhase == 2) {
                if (ListaItems.Instance.firstTimeInPhase2) {
                    gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);
                    canPlaySceneMusic = false;
                }
            }
            else if (gamePhase == 3) {
                if (ListaItems.Instance.firstTimeInPhase3) {
                    gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);
                    canPlaySceneMusic = false;
                }
            }
        }
        SoundController.GetInstance().LoadSounds();   //Carregando os sons da cena

        if (mainCamera != null)
            originalCameraSize = mainCamera.orthographicSize;
        if (scenariosListInOrder != null) {   //Aqui temos que popular a estrutura de cen�rios
            if(scenariosListInOrder.Length > 0) {
                scenarios = Scenarios.PopulateScenarios(scenariosListInOrder);
                scenarios.ScenariosDictionary[0].ScenarioObject.SetActive(true);   //Ativando o primeiro cen�rio

                isInMainScenario = true;
                currentScenario = scenarios.ScenariosDictionary[0];
                idActiveScenario = 0;    //Este � o �ndice do cen�rio que est� ativo no momento
                newScenario = null;
            }
        }

        if (Globals.firstScene)
            Globals.firstScene = false;
        else
            TransitionController.GetInstance().FadeOutScene();


        SoundController.GetInstance().ChangeVolumes(true);
        if (OSTVolumeSlider != null) {   //Se 1 slider estiver ativo, os outros tamb�m estar�o
            OSTVolumeSlider.value = Globals.volumeOST;
            SFXVolumeSlider.value = Globals.volumeSFX;
            OSTVolumeSlider.onValueChanged.AddListener((newValue) => {
                Globals.volumeOST = newValue;
                SoundController.GetInstance().ChangeVolumes(false);
            });
            SFXVolumeSlider.onValueChanged.AddListener((newValue) => {
                Globals.volumeSFX = newValue;
                SoundController.GetInstance().ChangeVolumes(false);
            });
        }
        if(canPlaySceneMusic)
            SoundController.GetInstance().PlaySceneMusic();
    }

    private void Update() {
        if(canvasPause != null) {
            if (Input.GetKeyDown(KeyCode.Escape) && !DialogueController.GetInstance().dialogueActive) {
                if (canvasConfigs.activeSelf)    //Se estiver no menu de configura��es
                    ExitConfigs();
                else {
                    if (gamePaused)
                        SoundController.GetInstance().ResumeCurrentTrack();
                    else
                        SoundController.GetInstance().PauseCurrentTrack();
                    canvasPause.SetActive(!gamePaused);
                    gamePaused = !gamePaused;
                }
            }
        }
    }

    public void ChangeScenarioButton(int direction) {    //Este m�todo servir� para trocar o cen�rio do jogo (quando olhamos para a esquerda/direita/cima)
        if (!isChangingScenario) {
            if (direction == (int)LookDirection.LEFT)
                SoundController.GetInstance().PlaySound("cena_esq");
            else if (direction == (int)LookDirection.RIGHT)
                SoundController.GetInstance().PlaySound("cena_dir");
            else if (direction == (int)LookDirection.BACK)
                SoundController.GetInstance().PlaySound("cena_back");

            DialogueController.GetInstance().EndDialogue();
            changeScenario(direction);
        }
    }

    public void changeScenario(int direction=(int)LookDirection.OTHER, string scenarioName=null) {
        if(direction != (int)LookDirection.OTHER) {   //Se estivermos virando para esquerda/direita/cima
            if (isInMainScenario) {    //Se estivermos em um cen�rio principal do jogo
                float horizontalOffset = 0, verticalOffset = 0;   //Estes valores s�o os campos do objeto do cen�rio que servir�o como offset da c�mera ao olhar para alguma dire��o
                switch (direction) {
                    case (int)LookDirection.LEFT:
                        if (idActiveScenario == 0)
                            idActiveScenario = scenarios.ScenariosDictionary.Count - 1;
                        else
                            idActiveScenario = idActiveScenario - 1;
                        horizontalOffset = -cameraOffset;
                        break;
                    case (int)LookDirection.RIGHT:
                        if (idActiveScenario == scenarios.ScenariosDictionary.Count - 1)
                            idActiveScenario = 0;
                        else
                            idActiveScenario = idActiveScenario + 1;
                        horizontalOffset = cameraOffset;
                        break;
                    case (int)LookDirection.UP:
                        if (idActiveScenario == scenarios.ScenariosDictionary.Count - 1)
                            idActiveScenario = 0;
                        else
                            idActiveScenario = idActiveScenario + 1;
                        verticalOffset = -cameraOffset;
                        break;
                }
                newScenario = scenarios.ScenariosDictionary[idActiveScenario];
                StartCoroutine(moveCameraOffset(horizontalOffset, verticalOffset));    //Novendo o cen�rio para dar a impress�o de movimenta��o da c�mera
            }
            else {
                newScenario = currentScenario.ParentScenario;
                idActiveScenario = newScenario.SceneId;
            }
        }
        else {    //Se clicarmos em uma parte do cen�rio
            if(scenarioName != null) {
                foreach(ScenarioDictionaryItem itemDict in currentScenario.PartsOfScenario) {
                    if (itemDict.ScenarioObject.name.Equals(scenarioName)) {
                        newScenario = itemDict;
                        idActiveScenario = itemDict.SceneId;
                        break;
                    }
                }
            }
        }
        isInMainScenario = newScenario.ParentScenario == null;
        TransitionController.GetInstance().LoadScenario();
    }

    public void changeImgScenario() {    //Este m�todo ser� chamado logo ap�s o fade-in da transi��o entre cen�rios
        currentScenario.ScenarioObject.SetActive(false);
        newScenario.ScenarioObject.SetActive(true);
        currentScenario = newScenario;
        newScenario = null;
        if (isInMainScenario) {
            btnBack.SetActive(false);
            btnLeft.SetActive(true);
            btnRight.SetActive(true);
        }
        else {
            btnBack.SetActive(true);
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
        }
    }

    private IEnumerator moveCameraOffset(float horizontalOffset, float verticalOffset) {
        isChangingScenario = true;
        canvasScenarios.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        StartCoroutine(moveCameraNearScenario());

        float timePassed = 0f, lerpDuration = 1f;
        RectTransform scenarioRectTransform = currentScenario.ScenarioObject.GetComponent<RectTransform>();
        while (timePassed < lerpDuration) {
            float t = timePassed / lerpDuration;
            float lerpValueLeft = Mathf.Lerp(scenarioRectTransform.offsetMin.x, -horizontalOffset, t);
            float lerpValueRight = Mathf.Lerp(scenarioRectTransform.offsetMax.x, -horizontalOffset, t);
            float lerpValueBottom = Mathf.Lerp(scenarioRectTransform.offsetMin.y, verticalOffset, t);
            float lerpValueTop = Mathf.Lerp(scenarioRectTransform.offsetMax.y, verticalOffset, t);
            scenarioRectTransform.offsetMin = new Vector2(lerpValueLeft, lerpValueBottom);
            scenarioRectTransform.offsetMax = new Vector2(lerpValueRight, lerpValueTop);
            timePassed += Time.deltaTime;
            yield return null;
        }
        scenarioRectTransform.offsetMin = Vector2.zero;
        scenarioRectTransform.offsetMax = Vector2.zero;
        isChangingScenario = false;
    }

    private IEnumerator moveCameraNearScenario() {
        float targetSize = mainCamera.orthographicSize * 0.95f;
        float timePassed = 0f, lerpDuration = 0.4f;
        while (timePassed < lerpDuration) {
            float t = timePassed / lerpDuration;
            float lerpValue = Mathf.Lerp(mainCamera.orthographicSize, targetSize, t);
            mainCamera.orthographicSize = lerpValue;
            timePassed += Time.deltaTime;
            yield return null;
        }
        mainCamera.orthographicSize = originalCameraSize;
        canvasScenarios.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
    }

    public void LoadQuarto() {
        TransitionController.GetInstance().LoadQuarto();
    }

    public void ResetGame() {   //Deve ser chamada quando voltamos ao menu ou zeramos o jogo
        ListaItems.Instance.resetVariables();
        DialogueController.GetInstance().dialogueVariablesController.ChangeSpecificVariable("resetDialogueVariables");
    }

    public void checkActionsAfterDialogue() {
        if (canvasCutsceneMusica.activeSelf) {
            canvasCutsceneMusica.SetActive(false);
            SoundController.GetInstance().ResumeCurrentTrack();
            if (ListaItems.Instance.musicaBaixo && ListaItems.Instance.musicaBateria && !ListaItems.Instance.exitKeyGenerated) {
                exitKey.SetActive(true);
                ListaItems.Instance.exitKeyGenerated = true;
                gameObject.GetComponent<DialogueTrigger>().TriggerExamDialogue(true);   //Chave
            }
        }
        canvasCutsceneMusica.SetActive(false);
        if (gamePhase == 1 && ListaItems.Instance.guitarraInteragida) {
            StartCoroutine(NextPhase());
        }
        if (gamePhase == 2 && ListaItems.Instance.musicaVocal && ListaItems.Instance.musicaTeclado) {
            StartCoroutine(NextPhase());
        }

        if(isInQuarto) {   //Diálogos iniciais de fase
            if (ListaItems.Instance.firstTimeInPhase1 && gamePhase == 1) {
                ListaItems.Instance.firstTimeInPhase1 = false;
                SoundController.GetInstance().PlaySceneMusic();
            }
            else if (ListaItems.Instance.firstTimeInPhase2 && gamePhase == 2) {
                ListaItems.Instance.firstTimeInPhase2 = false;
                SoundController.GetInstance().PlaySceneMusic();
            }
            else if (ListaItems.Instance.firstTimeInPhase3 && gamePhase == 3) {
                ListaItems.Instance.firstTimeInPhase3 = false;
                SoundController.GetInstance().PlaySceneMusic();
            }
        }
    }

    private IEnumerator NextPhase() {
        yield return new WaitForSeconds(0.5f);
        TransitionController.GetInstance().LoadNextFase();
    }

    public void changeTV() {
        if (TV_medo1.activeSelf) {
            TV_medo1.SetActive(false);
            TV_medo2.SetActive(true);
        }
        else if (TV_medo2.activeSelf) {
            TV_medo2.SetActive(false);
            TV_medo3.SetActive(true);
        }
        else if (TV_medo3.activeSelf) {
            TV_medo3.SetActive(false);
            TV_medo1.SetActive(true);
        }
    }

    //M�todos para bot�es do menu e do menu de pausa:
    public void StartGame() {
        SoundController.GetInstance().PlaySound("btn");
        TransitionController.GetInstance().LoadNextScene();
    }
    public void Creditos() {
        SoundController.GetInstance().PlaySound("btn");
        TransitionController.GetInstance().LoadCreditos();
    }
    public void QuitGame() {
        SoundController.GetInstance().PlaySound("btn");
        Application.Quit();
    }
    public void EnterConfigs() {
        SoundController.GetInstance().PlaySound("btn");
        if (canvasPause != null)
            canvasPause.SetActive(false);
        else if (canvasMenu != null)
            canvasMenu.SetActive(false);
        canvasConfigs.SetActive(true);
    }
    public void ExitConfigs() {
        //SoundController.GetInstance().PlaySound("btn");
        if (canvasPause != null)
            canvasPause.SetActive(true);
        else if (canvasMenu != null)
            canvasMenu.SetActive(true);
        canvasConfigs.SetActive(false);
    }
    public void ReturnToGameMenu() {   //Para retornar ao menu do jogo
        SoundController.GetInstance().PlaySound("btn");
        TransitionController.GetInstance().LoadMenu();
    }
    

}
