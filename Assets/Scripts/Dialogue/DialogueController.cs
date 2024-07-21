using Ink.Runtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{    //Esta classe ser� �nica para todo o projeto (singleton class)
    private static DialogueController instance;

    public TextAsset variablesJSON;    //Este � o arquivo JSON do ink que cont�m todas as vari�veis de di�logo
    public GameObject canvasDialogue, dialogueBox, bgDialogue;
    public TextMeshProUGUI txtDialogue;
    public DialogueVariablesController dialogueVariablesController { get; private set; }

    private Story dialogue;

    private bool endLine = false;   //Esta vari�vel � respons�vel por guardar se cada linha do di�logo j� terminou ou ainda n�o
    private bool letterEfect = true;   //Define se o di�logo ter� o efeito de letras aparecendo
    private float textDialogueSpeed;
    private int indexLine;

    public bool dialogueActive { get; private set; }   //Quero que esta vari�vel possa ser lida por outros scripts, mas n�o modificada
    public float showPanelDialogueTax = 9f, opacityPanelDialogue = 0.5f;

    public static DialogueController GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        //Ao carregar pela primeira vez, precisamos carregar as vari�veis criadas no ink para o c�digo. Fa�o isso chamando o pr�prio construtor da classe DialogueVariablesController:
        dialogueVariablesController = new DialogueVariablesController(variablesJSON);
    }

    void Start()
    {
        dialogueActive = false;
    }

    private void Update()
    {
        if (dialogueActive)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Space))
                PassDialogue();
        }
        if (endLine)
        {
            endLine = false;
        }
    }

    public IEnumerator ShowCanvasDialogue(float showPanelDialogueTax)
    {   //Este m�todo ser� respon�vel por mostrar o canvas de di�logo
        Image dialogueBoxRectTransform = dialogueBox.GetComponent<Image>();
        Color initialColor = new Color(0, 0, 0, 0);
        dialogueBoxRectTransform.color = initialColor;
        float targetValue = opacityPanelDialogue;   //Este � o valor desejado para a opacidade da caixa de di�logo
        bool txtStarted = false;
        while (Mathf.Abs(targetValue - dialogueBoxRectTransform.color.a) > 0.01f)
        {
            float lerpValue = Mathf.Lerp(dialogueBoxRectTransform.color.a, targetValue, showPanelDialogueTax * Time.deltaTime);
            Color newColor = new Color(0, 0, 0, lerpValue);
            dialogueBoxRectTransform.color = newColor;
            if (targetValue - Mathf.Abs(dialogueBoxRectTransform.color.a) < 0.015f && !txtStarted)
            {    //Para come�ar a mostrar as letras do di�logo um pouco antes de mostrar a caixa
                txtStarted = true;
                StartTextDialogue();
            }
            yield return null;
        }
    }

    public void StartDialogue(TextAsset dialogueJSON, float textSpeed, float fontSize, bool isInteractionDialogue)
    {
        dialogue = new Story(dialogueJSON.text);        //Carregando o di�logo a partir do arquivo JSON passado de par�metro
        textDialogueSpeed = textSpeed;
        txtDialogue.fontSize = fontSize;
        canvasDialogue.SetActive(true);
        letterEfect = isInteractionDialogue;
        //Image bgDialogueImg = bgDialogue.GetComponent<Image>();
        //bgDialogueImg.raycastTarget = true;
        GameController.GetInstance().blockActionsDialogue = true;

        if (isInteractionDialogue)
            StartCoroutine(ShowCanvasDialogue(showPanelDialogueTax));
        else
        {
            Image dialogueBoxRectTransform = dialogueBox.GetComponent<Image>();
            Color colorPanel = new Color(0, 0, 0, opacityPanelDialogue);
            dialogueBoxRectTransform.color = colorPanel;
            StartTextDialogue();
        }

    }

    public void StartTextDialogue()
    {
        dialogueActive = true;
        dialogueVariablesController.StartListening(dialogue);  //Para detectar as mudan�as de vari�veis no di�logo
        if (dialogue.canContinue)
        {
            dialogue.Continue();
            StartCoroutine(PrintDialogue());
        }
    }

    private void PassDialogue()
    {
        string fala = dialogue.currentText;

        if (indexLine < fala.Length - 1)
        {         //Se n�o estiver no final da fala
            StopAllCoroutines();
            indexLine = fala.Length - 1;
            endLine = true;
            txtDialogue.text = fala;
        }
        else
        {
            if (dialogue.currentChoices.Count == 0)
            {
                //SoundController.GetInstance().PlaySound("skip_dialogo", null);
                if (!dialogue.canContinue)     //Se estiver no final do di�logo
                    EndDialogue();
                else
                {
                    dialogue.Continue();
                    StartCoroutine(PrintDialogue());
                }
            }
        }
    }

    //Fun��o que printa cada linha do di�logo na caixa de di�logo
    private IEnumerator PrintDialogue()
    {
        string fala = dialogue.currentText;    //Pegando a fala atual do di�logo
        if (letterEfect)
        {
            txtDialogue.text = "";
            for (int i = 0; i < fala.Length; i++)
            {    //Fazendo as letras aparecerem uma de cada vez
                txtDialogue.text += fala[i];
                indexLine = i;
                yield return new WaitForSeconds(textDialogueSpeed);
            }
        }
        else
        {
            txtDialogue.text = fala;
            indexLine = fala.Length - 1;
        }
        endLine = true;
    }

    public void EndDialogue()
    {   //M�todo chamado ao fim do di�logo
        txtDialogue.text = "";
        canvasDialogue.SetActive(false);
        dialogueActive = false;
        if (dialogue != null)
            dialogueVariablesController.StopListening(dialogue);  //Para parar de detectar as mudan�as de vari�veis no di�logo
        GameController.GetInstance().blockActionsDialogue = false;
        //GameController.checkVariablesDialogue(dialogueVariablesController.variablesValues);    //Fazendo as checagens de vari�veis importantes que podem ter mudado ap�s um di�logo
    }

    public Ink.Runtime.Object GetVariableState(string variableName)
    {    //Esta fun��o servir� para recuperar o estado de determinada vari�vel de di�logo
        Ink.Runtime.Object variableValue = null;
        dialogueVariablesController.variablesValues.TryGetValue(variableName, out variableValue);
        if (variableValue == null)
        {
            Debug.Log("N�o foi poss�vel recuperar o valor da vari�vel de di�logo informada.");
            return null;
        }
        return variableValue;
    }
}
