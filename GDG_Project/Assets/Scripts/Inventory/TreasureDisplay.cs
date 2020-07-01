using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureDisplay : MonoBehaviour
{
    public Item_ScptObj item;
    public Color spriteColor;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if(!spriteRenderer) { return; }

        spriteRenderer.sprite = item.itemSprite;
        spriteRenderer.color = spriteColor;
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && InventoryManager.instance)
        {
            if (InventoryManager.instance.CanAddToInventory(item))
            {
                Destroy(gameObject);
            }
        }
    }
}
