using UnityEngine;

public class Object_Blacksmith : Object_NPC, IInteractble
{
    private Animator anim;
    private Inventory_Player inventory;
    private Inventory_Storage storage;

    protected override void Awake()
    {
        base.Awake();
        storage = GetComponent<Inventory_Storage>();
        anim = GetComponentInChildren<Animator>();
        anim.SetBool("IsBlacksmith", true);
    }
    public override void Interact()
    {
        base.Interact();
        ui.storageUI.SetupStorage(storage);
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
        ui.SwitchOffAllTooltips();
        ui.OpenStorageUI(false);
    }
}
