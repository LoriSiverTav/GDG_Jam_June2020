using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureDisplay : MonoBehaviour
{
    public Item_ScptObj item;
    public Color spriteColor;
    public KeyCode pickupKey;

    public Inventory playerInventory;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = GameObject.Find("tempPlayer").GetComponent<Inventory>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(!spriteRenderer) { return; }

        spriteRenderer.sprite = item.itemSprite;
        spriteRenderer.color = spriteColor;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(pickupKey) && playerInventory)
        {
            if (playerInventory.CanAddToInventory(item))
                Destroy(gameObject);
        }
    }
}
