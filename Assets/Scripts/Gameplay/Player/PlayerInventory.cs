using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerInventory
{
    [field: SerializeField] public List<Item> Inventory { get; private set; } = new List<Item>();

    //[field: SerializeField] public Equipment RightHand { get; private set; }
    //[field: SerializeField] public Equipment LeftHand { get; private set; }
    //[field: SerializeField] public Equipment head { get; private set; }
    //[field: SerializeField] public Equipment armour { get; private set; }
    //[field: SerializeField] public Equipment legs { get; private set; }
    //[field: SerializeField] public Equipment boots { get; private set; }

    public void PickupItem(Item item)
    {
        /*if (item.ItemData.itemType == ItemType.rightHand && RightHand.item == null)
        {
            RightHand.item = item;

            item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            item.transform.SetParent(RightHand.itemPlacement);

            //var pickedItem = UnityEngine.Object.Instantiate(item.ItemData.itemPrefab, RightHand.itemPlacement);
            //pickedItem.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            //pickedItem.transform.SetParent(RightHand.itemPlacement);
        }

        else
            Inventory.Add(item);*/

        Inventory.Add(item);
    }

    public void DropItem(Item item)
    {

    }
}

[Serializable]
public class Equipment
{
    public Item item;
    public Transform itemPlacement;
}