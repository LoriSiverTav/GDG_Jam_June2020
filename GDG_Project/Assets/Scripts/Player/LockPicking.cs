using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockPicking : MonoBehaviour
{
    public Canvas lockpickCanvas;
    public Canvas finalDialCanvas;
    public UILock levelLock;
    public bool canOpenUI = false;
    public bool canOpenFinalUI = false;

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("Lock"))
        {
            levelLock = GameObject.Find("Lock").GetComponent<UILock>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(canOpenUI && Input.GetKeyDown(KeyCode.E) && !LevelManager.instance.levels[LevelManager.instance.lvlIndex].isComplete)
        {
            PlayerMovement.isPuzzling = !PlayerMovement.isPuzzling;
            levelLock.ResetLock();
        }
        
        if(canOpenFinalUI && Input.GetKeyDown(KeyCode.E) && !LevelManager.instance.finalSolution.isSolved && InventoryManager.instance.treasurePieces.Count == 4)
        {
            PlayerMovement.isPuzzling = !PlayerMovement.isPuzzling;
        }

        if (lockpickCanvas)
        {
            lockpickCanvas.enabled = PlayerMovement.isPuzzling && DeathScreen.userTries > 0;
        }
        
        if (finalDialCanvas)
        {
            finalDialCanvas.enabled = PlayerMovement.isPuzzling;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            canOpenUI = true;
        }
        else if(collision.gameObject.tag == "FinalChest")
        {
            canOpenFinalUI = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Chest")
        {
            canOpenUI = false;
        }
        else if (collision.gameObject.tag == "FinalChest")
        {
            canOpenFinalUI = false;
        }
    }
}
