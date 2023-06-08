using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

[SelectionBase, DisallowMultipleComponent]
public class ClientCharacter : NetworkBehaviour, IDamageable
{
    public static HashSet<ClientCharacter> Players { get; private set; } = new HashSet<ClientCharacter>();

    [field: SerializeField] public PlayerInventory Inventory { get; private set; }
    [Space]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerData playerData;

    private float health;

    private void OnEnable() => Players.Add(this);
    private void OnDisable() => Players.Remove(this);

    private void Start()
    {
        health = playerData.baseHealth;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) playerCamera.gameObject.SetActive(false);

        name = $"Player {OwnerClientId}";
    }

    public void DealDamage(float damage)
    {
        if (!IsOwner) return;

        health -= damage;

        if (health <= 0f)
            Kill();
    }

    private void Kill()
    {

    }
}