public class ItemInventory {    //Esta classe representa o item no inventário
    public Item item;
    public DialogueTrigger dialogueTrigger;

    public ItemInventory(Item item, DialogueTrigger dialogueTrigger) {
        this.item = item;
        this.dialogueTrigger = dialogueTrigger;
    }

    public bool Equals(ItemInventory other) {
        if (other == null)
            return false;
        return this.item.id == other.item.id;
    }

    public override bool Equals(object obj) {
        if (obj == null) return false;
        if (obj.GetType() != this.GetType()) return false;
        return Equals(obj as ItemInventory);
    }

    public override int GetHashCode() {
        return (item.id).GetHashCode();
    }
}
