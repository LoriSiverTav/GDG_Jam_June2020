using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class LockTumbler
{
    public Image tumbler;
    public bool isPushed = false;
    public int value;

    [HideInInspector]
    public Vector3 initialLoc;
}

public class UILock : MonoBehaviour
{
    public LockTumbler[] tumblers;
    public Image lockPick;
    public int[] solution;
    public List<int> playerInput;
    public bool isUnlocked = false;
    public int tries = 3;

    private int targetTumbler = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Get lock solution of the level
        LevelSol levelData = LevelManager.instance.GetLevelData();
        solution = levelData.lockSolution;

        if (tumblers.Length == 0) { return; }

        // Save the initial location of tumblers for when resetting.
        for (int i = 0; i < tumblers.Length; i++)
        {
            tumblers[i].value = i;
            tumblers[i].initialLoc = tumblers[i].tumbler.gameObject.transform.localPosition;
        }

        // Position the lockpick underneath the first tumbler
        lockPick.gameObject.transform.position = new Vector3(
            tumblers[targetTumbler].tumbler.gameObject.transform.position.x,
            tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 60,
            0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PlayerMovement.isPuzzling || isUnlocked) { return; }

        if(tries <= 0) 
        {
            PlayerMovement.isPuzzling = false;
            SceneManager.LoadScene(1);
            return; 
        }

        // Move the lockpick to the right
        if(Input.GetKeyDown(KeyCode.D))
        {
            // Go back to the first tumbler when trying to move beyond the last tumbler
            targetTumbler = targetTumbler + 1 < tumblers.Length ? targetTumbler + 1 : 0;

            // Move lockpick to the right
            lockPick.gameObject.transform.position =
            new Vector3(
                tumblers[targetTumbler].tumbler.gameObject.transform.position.x,
                tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 60,
                0);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            PushTumbler();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetLock();
        }

        if(playerInput.Count == solution.Length)
        {
            if(IsLockIsComplete())
            {
                Debug.Log("Lock is Unlocked");
                isUnlocked = true;
                playerInput.Clear();
                PlayerMovement.isPuzzling = false;
            }
            else
            {
                tries--;

                if(tries <= 0)
                {
                    Debug.Log("Player dead");
                }
            }
            
            ResetLock();
        }
    }

    public void ResetLock()
    {
        foreach(var sol in tumblers)
        {
            sol.isPushed = false;
            sol.tumbler.transform.localPosition =
                new Vector3(sol.initialLoc.x, sol.initialLoc.y, sol.initialLoc.z);
        }

        targetTumbler = 0;

        lockPick.gameObject.transform.position =
            new Vector3(
                tumblers[targetTumbler].tumbler.gameObject.transform.position.x,
                tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 60,
                0);

        playerInput.Clear();

    }

    private bool IsLockIsComplete()
    {
        foreach (var i in playerInput)
        {
            if (playerInput[i] != solution[i])
            {
                return false;
            }
        }

        return true;
    }

    private void PushTumbler()
    {
        LockTumbler tumblerSol = tumblers[targetTumbler];

        if (tumblerSol.isPushed) { return; }

        // Move the lockpick upwards
        lockPick.gameObject.transform.position =
        new Vector3(
            lockPick.gameObject.transform.position.x,
            tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 30,
            0);

        // Move the tumbler upwards
        tumblerSol.tumbler.transform.localPosition =
        new Vector3(
            tumblerSol.tumbler.gameObject.transform.localPosition.x,
            -25f,
            tumblerSol.tumbler.gameObject.transform.localPosition.z);

        // Update the player input list and tumbler status
        tumblerSol.isPushed = true;
        playerInput.Add(tumblerSol.value);
    }
}
