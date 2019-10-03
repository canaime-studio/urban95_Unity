using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace MobileVRInventory
{
    public class TesteScript : MonoBehaviour
    {
        public VRInventory VRInventory;

        public void OnItemSelected(InventoryItemStack stack)
        {
            Debug.Log(stack.item.name);
            VRInventory.RemoveItem("Balde", 1, stack);
            //UpdateBars();
        }

        public void OnItemPickedUp(VRInventory.InventoryItemPickupResult result)
        {
            Debug.Log(result.item.name);
        }
    }
}

