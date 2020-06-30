using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockPicking : MonoBehaviour
{
    public Canvas lockpickCanvas;
    public UILock levelLock;
    public bool canOpenUI = false;

    // Start is called before the first frame update
    void Start()
    {
        levelLock = GameObject.Find("Lock").GetComponent<UILock>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpenUI && Input.GetKeyDown(KeyCode.E) && !levelLock.isUnlocked)
        {
            PlayerMovement.isPuzzling = !PlayerMovement.isPuzzling;
            levelLock.ResetLock();
        }
        
        if(lockpickCanvas)
        {
            lockpickCanvas.enabled = PlayerMovement.isPuzzling;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            canOpenUI = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            canOpenUI = false;
        }
    }
}
