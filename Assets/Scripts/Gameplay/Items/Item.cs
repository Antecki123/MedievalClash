using Unity.Netcode;
using UnityEngine;

[SelectionBase, DisallowMultipleComponent]
public class Item : NetworkBehaviour, IInteractable
{
    [field: SerializeField] public ItemData ItemData { get; private set; }

    private BoxCollider itemCollider;
    private Outline outline;

    private void Awake()
    {
        itemCollider = GetComponent<BoxCollider>();
        outline = GetComponent<Outline>();
    }

    private void Start()
    {
        itemCollider.center = ItemData.colliderCenter;
        itemCollider.size = ItemData.colliderSize;

        name = ItemData.name;
        outline.enabled = false;
    }

    private void OnMouseOver() => outline.enabled = true;
    private void OnMouseExit() => outline.enabled = false;

    public void Interact(PlayerNetwork player)
    {
        outline.enabled = false;
        player.Inventory.PickupItem(this);

        PickUpItemServerRPC();
    }

    [ServerRpc(RequireOwnership = false)]
    private void PickUpItemServerRPC()
    {
        PickUpItemClientRPC();
    }

    [ClientRpc]
    private void PickUpItemClientRPC()
    {
        //Destroy(gameObject);
    }
}

public enum ItemType
{
    none,
    rightHand,
    leftHand,
    head,
    armour,
    legs,
    boots,
}