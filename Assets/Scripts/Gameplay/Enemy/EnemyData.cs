using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Objects/Objects Data/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;

    [Header("Movement")]
    public float velocity = 5f;

    [Header("Persistence")]
    public float baseHealth = 100f;

    [Header("Attack")]
    public float surveillanceRange = 5.0f;
    public float attackRange = 1f;
    public float attackCooldown = 2.0f;
}