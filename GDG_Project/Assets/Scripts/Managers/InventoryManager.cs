using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public int InventorySize = 4;
    public List<Item_ScptObj> treasurePieces;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(instance.treasurePieces.Count);
        }
    }

    public bool CanAddToInventory(Item_ScptObj newItem)
    {
        if (instance.treasurePieces.Count >= instance.InventorySize)
        {
            Debug.LogError("Inventory Full");
            return false;
        }

        instance.treasurePieces.Add(newItem);
        return true;
    }

    public void UpdateInventoryUI()
    {

    }
}
