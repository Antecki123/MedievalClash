using TMPro;
using Unity.Netcode;
using UnityEngine;

[SelectionBase, DisallowMultipleComponent]
public class PlayerNetwork : NetworkBehaviour, IDamageable
{
    //public static HashSet<PlayerNetwork> Players { get; private set; } = new HashSet<PlayerNetwork>();
    [field: SerializeField] public PlayerInventory Inventory { get; private set; }
    [Space]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayerData playerData;

    [SerializeField] private TextMeshProUGUI playerIDLabel; //DEBUG

    private float health;

    private void Start()
    {
        health = playerData.baseHealth;
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) playerCamera.gameObject.SetActive(false);

        name = $"Player {OwnerClientId}";

        playerIDLabel.text = OwnerClientId.ToString();
    }

    private void LateUpdate()
    {
        playerIDLabel.transform.LookAt(playerCamera.transform.position);
        playerIDLabel.transform.Rotate(new Vector3(0f, 180f, 0f));
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