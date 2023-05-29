using Unity.Netcode;
using UnityEngine;

[SelectionBase, DisallowMultipleComponent, RequireComponent(typeof(Outline))]
public class Chest : NetworkBehaviour, IInteractable
{
    [SerializeField] private Transform spawnTransform;
    [Space]
    [SerializeField] private Item item;

    private Outline outline;

    private Animator animator;
    private readonly int openState = Animator.StringToHash("open");

    private NetworkVariable<bool> netIsOpen = new NetworkVariable<bool>(false);

    private void Awake()
    {
        outline = GetComponent<Outline>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        outline.enabled = false;
    }

    private void OnMouseOver() => outline.enabled = !netIsOpen.Value;
    private void OnMouseExit() => outline.enabled = false;

    public void Interact(PlayerNetwork player)
    {
        if (netIsOpen.Value) return;

        OpenChestServerRPC();
    }

    public override void OnNetworkSpawn()
    {
        if (netIsOpen.Value)
            animator.SetTrigger(openState);
    }

    [ServerRpc(RequireOwnership = false)]
    private void OpenChestServerRPC()
    {
        if (item != null)
        {
            var spawned = Instantiate(item, spawnTransform);
            spawned.GetComponent<NetworkObject>().Spawn(true);
        }

        netIsOpen.Value = true;
        OpenChestClientRpc();
    }

    [ClientRpc]
    private void OpenChestClientRpc()
    {
        animator.SetTrigger(openState);
    }
}