using System;
using System.Collections;
using UnityEngine;

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

    [SerializeField] private GameObject canvasPause, btnLeft, btnRight, btnUp, btnBack;
    [SerializeField] private float cameraOffset;

    private int idActiveScenario;
    private bool isChangingScenario = false, gamePaused=false, isInMainScenario;


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
        if (scenariosListInOrder.Length > 0) {   //Aqui temos que popular a estrutura de cenários
            scenarios = Scenarios.PopulateScenarios(scenariosListInOrder);
            scenarios.ScenariosDictionary[0].ScenarioObject.SetActive(true);   //Ativando o primeiro cenário

            isInMainScenario = true;
            currentScenario = scenarios.ScenariosDictionary[0];
            originalMainScenario = scenarios.ScenariosDictionary[0];
            idActiveScenario = 0;    //Este é o índice do cenário que está ativo no momento
            newScenario = null;
        }

        TransitionController.GetInstance().FadeOutScene();   //O fadeOut da cena só acontecerá depois de tudo que foi feito antes
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {    //Detectando "pause" do jogo
            if (canvasPause != null) {
                if (gamePaused)
                    canvasPause.SetActive(false);
                else
                    canvasPause.SetActive(true);
                gamePaused = !gamePaused;
            }
        }
    }

    public void ChangeSceneButton() {
        TransitionController.GetInstance().LoadNextScene();
    }

    public void ChangeScenarioButton(int direction) {    //Este método servirá para trocar o cenário do jogo (quando olhamos para a esquerda/direita/cima)
        string direcao = Enum.GetName(typeof(LookDirection), (LookDirection)direction);
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
            btnUp.SetActive(true);
        }
        else {
            btnBack.SetActive(true);
            btnLeft.SetActive(false);
            btnRight.SetActive(false);
            btnUp.SetActive(false);
        }
    }

    private IEnumerator moveCameraOffset(float horizontalOffset, float verticalOffset) {
        isChangingScenario = true;
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
}
