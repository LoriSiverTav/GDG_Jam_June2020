using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public int InventorySize;
    public List<Item_ScptObj> treasurePieces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanAddToInventory(Item_ScptObj newItem)
    {
        if (treasurePieces.Count >= InventorySize)
        {
            Debug.LogError("Inventory Full");
            return false;
        }

        treasurePieces.Add(newItem);
        return true;
    }
}
