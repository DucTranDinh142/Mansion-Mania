using UnityEngine;

public class Object_Smith : Object_NPC, IInteractable
{

    private Inventory_Player inventory;
    private Inventory_Storage storage;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        inventory = GetComponent<Inventory_Player>();
    }
    public void Interact()
    {
        ui.storageUI.SetupStorageUI(storage);
        ui.craftUI.SetupCraftUI(storage);

        ui.OpenStorageUI(true);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        inventory = player.GetComponent<Inventory_Player>();
        storage.SetInventory(inventory);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
        ui.HideAllToolTips();
        ui.OpenStorageUI(false);
    }
}
