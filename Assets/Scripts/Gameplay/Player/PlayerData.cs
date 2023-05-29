using UnityEngine;

[CreateAssetMenu(fileName = "Character Data", menuName = "Scriptable Objects/Objects Data/Character Data")]
public class PlayerData : ScriptableObject
{
    [Header("Movement")]
    public float velocity = 5f;
    public float turnSmooth = 0.1f;

    [Header("Persistence")]
    public float baseHealth = 100f;

    [Header("Interactions")]
    public float interactionRange = 1f;
    [Space]
    public float meleeAttackStrength = 5f;

}