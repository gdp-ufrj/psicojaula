using UnityEngine;

public class DialogueTrigger : MonoBehaviour {     //Este script apenas é responsável por triggar um diálogo quando alguma ação acontecer durante o jogo

    public TextAsset interactionDialogueJSON, examDialogueJSON;
    public float textDialogueSpeed = 0.05f, fontSize = 20f;

    public void TriggerInteractionDialogue() {
        if (!DialogueController.GetInstance().dialogueActive)
            DialogueController.GetInstance().StartDialogue(interactionDialogueJSON, textDialogueSpeed, fontSize, true);
    }

    public void TriggerExamDialogue() {
        DialogueController.GetInstance().StartDialogue(examDialogueJSON, textDialogueSpeed, fontSize, false);
    }

    public void SetVariables(DialogueTrigger dialogueTrigger) {
        this.interactionDialogueJSON = dialogueTrigger.interactionDialogueJSON;
        this.examDialogueJSON = dialogueTrigger.examDialogueJSON;
        this.textDialogueSpeed = dialogueTrigger.textDialogueSpeed;
        this.fontSize = dialogueTrigger.fontSize;
    }
}