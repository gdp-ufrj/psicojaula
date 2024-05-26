using UnityEngine;

public class DialogueTrigger : MonoBehaviour {     //Este script apenas é responsável por triggar um diálogo quando alguma ação acontecer durante o jogo

    public TextAsset dialogueJSON;
    [SerializeField] private float textDialogueSpeed = 0.05f;
    [SerializeField] private float fontSize = 20f;

    public void TriggerDialogue() {
        if (!DialogueController.GetInstance().dialogueActive)
            DialogueController.GetInstance().StartDialogue(dialogueJSON, textDialogueSpeed, fontSize);
    }
}