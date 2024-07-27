using UnityEngine;

[CreateAssetMenu(fileName= "New Item", menuName= "Item/Create New Item")]

public class Item:ScriptableObject
{
    public int id;
    public string itemName;
    public int value;
    public Sprite icon;
    //public Item newItem;
    public string nameSoundPickup;
    public bool isUsable;
    public GameObject newItemPrefab;
    public TextAsset[] interactionDialogueJSON, examDialogueJSON;
    
}
