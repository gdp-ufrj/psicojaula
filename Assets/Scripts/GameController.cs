using System;
using System.Collections;
using UnityEngine;
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

    private ScenarioDictionary scenarios;   //Todos os cenários do jogo (todos começarão desativados)  (não é possível serializar)
    private ScenarioDictionaryItem currentScenario, newScenario, originalMainScenario;

    [SerializeField] private GameObject[] scenariosListInOrder;    //Essa será uma lista serializada, na qual poderemos colocar os GameObjects de cenário

    [SerializeField] private GameObject canvasScenarios, btnLeft, btnRight, btnUp, btnBack;
    [SerializeField] private GameObject canvasPause, canvasMenu, canvasConfigs;  //Diferentes interfaces do jogo
    [SerializeField] private Slider OSTVolumeSlider, SFXVolumeSlider;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cameraOffset;

    private int idActiveScenario;
    private bool isChangingScenario = false, gamePaused=false, isInMainScenario;
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

    private void Start() {
        //Debug.Log(scenariosListInOrder);
        SoundController.GetInstance().LoadSounds();   //Carregando os sons da cena

        if (mainCamera != null)
            originalCameraSize = mainCamera.orthographicSize;
        if (scenariosListInOrder != null) {   //Aqui temos que popular a estrutura de cenários
            if(scenariosListInOrder.Length > 0) {
                scenarios = Scenarios.PopulateScenarios(scenariosListInOrder);
                scenarios.ScenariosDictionary[0].ScenarioObject.SetActive(true);   //Ativando o primeiro cenário

                isInMainScenario = true;
                currentScenario = scenarios.ScenariosDictionary[0];
                originalMainScenario = scenarios.ScenariosDictionary[0];
                idActiveScenario = 0;    //Este é o índice do cenário que está ativo no momento
                newScenario = null;
            }
        }

        if (Globals.firstScene)    //Se o jogo tiver acabado de abrir
            Globals.firstScene = false;
        else
            TransitionController.GetInstance().FadeOutScene();   //O fadeOut da cena só acontecerá depois de tudo que foi feito antes


        SoundController.GetInstance().ChangeVolumes(true);
        if (OSTVolumeSlider != null) {   //Se 1 slider estiver ativo, os outros também estarão
            //updateConfigs();
            OSTVolumeSlider.value = Globals.volumeOST;
            SFXVolumeSlider.value = Globals.volumeSFX;
            OSTVolumeSlider.onValueChanged.AddListener((newValue) => {
                Debug.Log(newValue);
                Globals.volumeOST = newValue;
                SoundController.GetInstance().ChangeVolumes(false);
            });
            SFXVolumeSlider.onValueChanged.AddListener((newValue) => {
                Globals.volumeSFX = newValue;
                SoundController.GetInstance().ChangeVolumes(false);
            });
        }
        SoundController.GetInstance().PlaySceneMusic();
        //DialogueController.GetInstance().dialogueVariablesController.CheckVariableValues();
    }

    private void Update() {
        if(canvasPause != null) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (canvasConfigs.activeSelf)    //Se estiver no menu de configurações
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

    public void ChangeScenarioButton(int direction) {    //Este método servirá para trocar o cenário do jogo (quando olhamos para a esquerda/direita/cima)
        changeScenario(direction);
    }

    public void changeScenario(int direction=(int)LookDirection.OTHER, string scenarioName=null) {
        if (!isChangingScenario) {
            if(direction != (int)LookDirection.OTHER) {   //Se estivermos virando para esquerda/direita/cima
                if (isInMainScenario) {    //Se estivermos em um cenário principal do jogo
                    float horizontalOffset = 0, verticalOffset = 0;   //Estes valores são os campos do objeto do cenário que servirão como offset da câmera ao olhar para alguma direção
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
                    originalMainScenario = newScenario;
                    StartCoroutine(moveCameraOffset(horizontalOffset, verticalOffset));    //Novendo o cenário para dar a impressão de movimentação da câmera
                }
                else {
                    newScenario = currentScenario.ParentScenario;
                    idActiveScenario = newScenario.SceneId;
                    //newScenario = originalMainScenario;
                    //idActiveScenario = originalMainScenario.SceneId;
                }
            }
            else {    //Se clicarmos em uma parte do cenário
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
    }

    public void changeImgScenario() {    //Este método será chamado logo após o fade-in da transição entre cenários
        currentScenario.ScenarioObject.SetActive(false);
        newScenario.ScenarioObject.SetActive(true);
        currentScenario = newScenario;
        newScenario = null;
        if (isInMainScenario) {
            btnBack.SetActive(false);
            btnLeft.SetActive(true);
            btnRight.SetActive(true);
            //btnUp.SetActive(true);
        }
        else {
            btnBack.SetActive(true);
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
            //btnUp.SetActive(false);
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

    public void changeOST1() {
        SoundController.GetInstance().PlaySound("OST_teste");
    }
    public void changeOST2() {
        SoundController.GetInstance().PlaySound("OST_house");
    }



    //Métodos para botões do menu e do menu de pausa:
    public void StartGame() {
        TransitionController.GetInstance().LoadNextScene();
    }
    public void QuitGame() {
        Application.Quit();
    }
    public void EnterConfigs() {
        if(canvasPause != null)
            canvasPause.SetActive(false);
        else if(canvasMenu != null)
            canvasMenu.SetActive(false);
        canvasConfigs.SetActive(true);
    }
    public void ExitConfigs() {
        if (canvasPause != null)
            canvasPause.SetActive(true);
        else if (canvasMenu != null)
            canvasMenu.SetActive(true);
        canvasConfigs.SetActive(false);
    }
    public void ReturnToGameMenu() {   //Para retornar ao menu do jogo
        TransitionController.GetInstance().LoadMenu();
    }

}
