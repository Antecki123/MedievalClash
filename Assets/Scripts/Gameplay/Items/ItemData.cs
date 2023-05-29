using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable Objects/Objects Data/New Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    [Space]
    public Vector3 colliderCenter;
    public Vector3 colliderSize;
}