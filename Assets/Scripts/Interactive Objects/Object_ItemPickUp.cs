using UnityEngine;

public class Object_ItemPickup : MonoBehaviour
{
    [SerializeField] private Vector2 dropForce = new Vector2(3, 10);
    [SerializeField] private Item_DataSO itemData;
    [Space]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Rigidbody2D itemRigidbody;
    [SerializeField] private Collider2D itemCollider2D;
    private void OnValidate()
    {
        if (itemData == null) return;
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetupVisuals();
    }

    public void SetupItem(Item_DataSO itemData)
    {
        this.itemData = itemData;
        SetupVisuals();

        float xDropForce = Random.Range(-dropForce.x, dropForce.x);
        itemRigidbody.linearVelocity = new Vector2(xDropForce, dropForce.y);
        itemCollider2D.isTrigger = false;
    }

    private void SetupVisuals()
    {
        spriteRenderer.sprite = itemData.itemIcon;
        gameObject.name = "Object_ItemPickUp - " + itemData.itemName;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground") && itemCollider2D.isTrigger == false)
        {
            itemCollider2D.isTrigger = true;
            itemRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Inventory_Player inventory = collision.GetComponent<Inventory_Player>();

        if(inventory == null) return;

        Inventory_Item itemToAdd = new Inventory_Item(itemData);
        Inventory_Storage storage = inventory.storage;

        if(itemData.itemType == ItemType.Material)
        {
            storage.AddMaterialToStash(itemToAdd);
            Destroy(gameObject);
            return;
        }

        if (inventory.CanAddItem(itemToAdd))
        {
            inventory.AddItem(itemToAdd);
            Destroy(gameObject);
        }
    }
}
