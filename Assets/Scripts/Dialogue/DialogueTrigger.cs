using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{     //Este script apenas � respons�vel por triggar um di�logo quando alguma a��o acontecer durante o jogo

    public TextAsset[] interactionDialogueJSON, examDialogueJSON;
    public float textDialogueSpeed = 0.05f, fontSize = 16f;

    public void TriggerInteractionDialogue(bool letterEfect, int idDialogue = -1, bool isCutsceneMusica=false)
    {
        if (!DialogueController.GetInstance().dialogueActive) {
            idDialogue = idDialogue == -1 ? 0 : idDialogue;
            if (interactionDialogueJSON[idDialogue] != null) {
                DialogueController.GetInstance().StartDialogue(interactionDialogueJSON[idDialogue], textDialogueSpeed, fontSize, letterEfect, isCutsceneMusica);
            }
        }
    }

    public void TriggerExamDialogue(bool letterEfect, int idDialogue = -1)
    {
        idDialogue = idDialogue == -1 ? 0 : idDialogue;
        if (examDialogueJSON[idDialogue] != null) {
            DialogueController.GetInstance().StartDialogue(examDialogueJSON[idDialogue], textDialogueSpeed, fontSize, letterEfect, false);
        }
    }

    public void SetVariables(DialogueTrigger dialogueTrigger)
    {
        this.interactionDialogueJSON = dialogueTrigger.interactionDialogueJSON;
        this.examDialogueJSON = dialogueTrigger.examDialogueJSON;
        this.textDialogueSpeed = dialogueTrigger.textDialogueSpeed;
        this.fontSize = dialogueTrigger.fontSize;
    }
}