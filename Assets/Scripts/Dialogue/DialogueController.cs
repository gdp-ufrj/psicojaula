using Ink.Runtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour {    //Esta classe será única para todo o projeto (singleton class)
    private static DialogueController instance;

    public TextAsset variablesJSON;    //Este é o arquivo JSON do ink que contém todas as variáveis de diálogo
    public GameObject canvasDialogue, dialogueBox, bgDialogue;
    public TextMeshProUGUI txtDialogue;
    public DialogueVariablesController dialogueVariablesController { get; private set; }

    private Story dialogue;

    private bool endLine = false;   //Esta variável é responsável por guardar se cada linha do diálogo já terminou ou ainda não
    private bool letterEfect = true;   //Define se o diálogo terá o efeito de letras aparecendo
    private float textDialogueSpeed;
    private int indexLine;

    public bool dialogueActive { get; private set; }   //Quero que esta variável possa ser lida por outros scripts, mas não modificada
    public float showPanelDialogueTax = 9f, opacityPanelDialogue=0.5f;

    public static DialogueController GetInstance() {
        return instance;
    }

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //Ao carregar pela primeira vez, precisamos carregar as variáveis criadas no ink para o código. Faço isso chamando o próprio construtor da classe DialogueVariablesController:
        dialogueVariablesController = new DialogueVariablesController(variablesJSON);
    }

    void Start() {
        dialogueActive = false;
    }

    private void Update() {
        if (dialogueActive) {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
                PassDialogue();
        }
        if (endLine) {
            endLine = false;
        }
    }

    public IEnumerator ShowCanvasDialogue(float showPanelDialogueTax) {   //Este método será responável por mostrar o canvas de diálogo
        Image dialogueBoxRectTransform = dialogueBox.GetComponent<Image>();
        Color initialColor = new Color(0, 0, 0, 0);
        dialogueBoxRectTransform.color = initialColor;
        float targetValue = opacityPanelDialogue;   //Este é o valor desejado para a opacidade da caixa de diálogo
        bool txtStarted = false;
        while (Mathf.Abs(targetValue - dialogueBoxRectTransform.color.a) > 0.01f) {
            float lerpValue = Mathf.Lerp(dialogueBoxRectTransform.color.a, targetValue, showPanelDialogueTax * Time.deltaTime);
            Color newColor = new Color(0, 0, 0, lerpValue);
            dialogueBoxRectTransform.color = newColor;
            if (targetValue - Mathf.Abs(dialogueBoxRectTransform.color.a) < 0.015f && !txtStarted) {    //Para começar a mostrar as letras do diálogo um pouco antes de mostrar a caixa
                txtStarted = true;
                StartTextDialogue();
            }
            yield return null;
        }
    }

    public void StartDialogue(TextAsset dialogueJSON, float textSpeed, float fontSize, bool isInteractionDialogue) {
        dialogue = new Story(dialogueJSON.text);        //Carregando o diálogo a partir do arquivo JSON passado de parâmetro
        textDialogueSpeed = textSpeed;
        txtDialogue.fontSize = fontSize;
        canvasDialogue.SetActive(true);
        letterEfect = isInteractionDialogue;
        //Image bgDialogueImg = bgDialogue.GetComponent<Image>();
        //bgDialogueImg.raycastTarget = true;
        GameController.GetInstance().blockActionsDialogue = true;

        if (isInteractionDialogue)
            StartCoroutine(ShowCanvasDialogue(showPanelDialogueTax));
        else {
            Image dialogueBoxRectTransform = dialogueBox.GetComponent<Image>();
            Color colorPanel = new Color(0, 0, 0, opacityPanelDialogue);
            dialogueBoxRectTransform.color = colorPanel;
            StartTextDialogue();
        }

    }

    public void StartTextDialogue() {
        dialogueActive = true;
        dialogueVariablesController.StartListening(dialogue);  //Para detectar as mudanças de variáveis no diálogo
        if (dialogue.canContinue) {
            dialogue.Continue();
            StartCoroutine(PrintDialogue());
        }
    }

    private void PassDialogue() {
        string fala = dialogue.currentText;

        if (indexLine < fala.Length - 1) {         //Se não estiver no final da fala
            StopAllCoroutines();
            indexLine = fala.Length - 1;
            endLine = true;
            txtDialogue.text = fala;
        }
        else {
            if (dialogue.currentChoices.Count == 0) {
                //SoundController.GetInstance().PlaySound("skip_dialogo", null);
                if (!dialogue.canContinue)     //Se estiver no final do diálogo
                    EndDialogue();
                else {
                    dialogue.Continue();
                    StartCoroutine(PrintDialogue());
                }
            }
        }
    }

    //Função que printa cada linha do diálogo na caixa de diálogo
    private IEnumerator PrintDialogue() {
        string fala = dialogue.currentText;    //Pegando a fala atual do diálogo
        if (letterEfect) {
            txtDialogue.text = "";
            for (int i = 0; i < fala.Length; i++) {    //Fazendo as letras aparecerem uma de cada vez
                txtDialogue.text += fala[i];
                indexLine = i;
                yield return new WaitForSeconds(textDialogueSpeed);
            }
        }
        else {
            txtDialogue.text = fala;
            indexLine = fala.Length - 1;
        }
        endLine = true;
    }

    public void EndDialogue() {   //Método chamado ao fim do diálogo
        txtDialogue.text = "";
        canvasDialogue.SetActive(false);
        dialogueActive = false;
        if(dialogue != null)
            dialogueVariablesController.StopListening(dialogue);  //Para parar de detectar as mudanças de variáveis no diálogo
        GameController.GetInstance().blockActionsDialogue = false;
        //GameController.checkVariablesDialogue(dialogueVariablesController.variablesValues);    //Fazendo as checagens de variáveis importantes que podem ter mudado após um diálogo
    }

    public Ink.Runtime.Object GetVariableState(string variableName) {    //Esta função servirá para recuperar o estado de determinada variável de diálogo
        Ink.Runtime.Object variableValue = null;
        dialogueVariablesController.variablesValues.TryGetValue(variableName, out variableValue);
        if (variableValue == null) {
            Debug.Log("Não foi possível recuperar o valor da variável de diálogo informada.");
            return null;
        }
        return variableValue;
    }
}
