using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

enum horizontalDirection { left, right };

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
   
    public Text tryText;

    public LockTumbler[] tumblers;
    public Image lockPick;
    public List<int> playerInput;
    public int tries = 3;
    public float lerpSpeed;
    public float tumblerPushHeight = 15;

    private int[] lockSolution;
    private int targetTumbler = 0;

    // Start is called before the first frame update
    void Start()
    {
       

        // Get lock solution of the level
        LevelSol levelData = LevelManager.instance.GetLevelData();
        lockSolution = levelData.lockSolution;

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
            tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 30,
            0);
    }

    // Update is called once per frame
    void Update()
    {
        // "Blinking" tumblers - Reset back to white
        ResetTumblerColors();

        // Only use the lockpicking controls when IsPuzzling and when the door is locked
        if (!PlayerMovement.isPuzzling || LevelManager.instance.levels[LevelManager.instance.lvlIndex].isComplete) 
        { 
            return; 
        }

        // Move the lockpick to the right
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveLockpickHorizontal(horizontalDirection.right);
        }

        // Move the lockpick to the left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLockpickHorizontal(horizontalDirection.left);
        }

        // Move the lockpick upwards
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            PushTumbler();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetLock();
        }
        
        CheckSolution();

    }

    public void ResetLock()
    {
        // Reset the tumbler's isPushed status to false, and position to down
        foreach(var sol in tumblers)
        {
            sol.isPushed = false;
            sol.tumbler.transform.localPosition =
                new Vector3(sol.initialLoc.x, sol.initialLoc.y, sol.initialLoc.z);
        }

        // Move the lockpick back to underneath the first tumbler
        targetTumbler = 0;

        lockPick.gameObject.transform.position =
            new Vector3(
                tumblers[targetTumbler].tumbler.gameObject.transform.position.x,
                tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 30,
                0);

        playerInput.Clear();
    }

    private bool IsLockIsComplete()
    {
        bool isCorrect = true;

        for(int i = 0; i < tumblers.Length; i++)
        {
            // Ensures to update the status only once - when the player input does not match the solution
            if(isCorrect)
            {
                if (lockSolution[i] != playerInput[i])
                    isCorrect = false;
            }

            // Blink the correct color on the tumbler - Depending if it was pressed in the correct order
            tumblers[playerInput[i]].tumbler.color = 
                lockSolution[i] == playerInput[i] ? Color.green : Color.red;
        }

        return isCorrect;
    }

    private void PushTumbler()
    {
        LockTumbler tumblerSol = tumblers[targetTumbler];

        if (tumblerSol.isPushed) { return; }

        // Move the lockpick upwards
        lockPick.gameObject.transform.position =
        new Vector3(
            lockPick.gameObject.transform.position.x,
            tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 20,
            0);

        // Move the tumbler upwards
        tumblerSol.tumbler.transform.localPosition =
        new Vector3(
            tumblerSol.tumbler.gameObject.transform.localPosition.x,
            tumblerPushHeight,
            tumblerSol.tumbler.gameObject.transform.localPosition.z);

        // Update the player input list and tumbler status
        tumblerSol.isPushed = true;
        playerInput.Add(tumblerSol.value);
    }

    private void ResetTumblerColors()
    {
        // Every tumbler will go from Red/Green to white
        foreach (var t in tumblers)
        {
            t.tumbler.color = Color.Lerp(t.tumbler.color, Color.white, Time.deltaTime * lerpSpeed);
        }
    }

    private void MoveLockpickHorizontal(horizontalDirection direction)
    {
        // Update the target tumbler value to loop when attempting to go beyond either side
        switch(direction)
        {
            case horizontalDirection.left:
                targetTumbler = targetTumbler - 1 < 0 ? tumblers.Length - 1 : targetTumbler - 1; 
                break;
            case horizontalDirection.right:
                targetTumbler = targetTumbler + 1 < tumblers.Length ? targetTumbler + 1 : 0;
                break;
        }

        // Move lockpick horizontally
        lockPick.gameObject.transform.position =
        new Vector3(
            tumblers[targetTumbler].tumbler.gameObject.transform.position.x,
            tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 30,
            0);
    }

    public void CheckSolution()
    {
        if (playerInput.Count == lockSolution.Length)
        {
            if (IsLockIsComplete())
            {
                Debug.Log("Lock is Unlocked");
                LevelManager.instance.levels[LevelManager.instance.lvlIndex].isComplete = true;
                PlayerMovement.isPuzzling = false;
            }
            else
            {
                tries--;
                setTryText();

                if (tries <= 0)
                {
                    Debug.Log("Player dead");
                    PlayerMovement.isPuzzling = false;
                    DeathScreen.userTries = 0;
                }
            }

            ResetLock();
        }
    }

    public void setTryText()
    {
        tryText.text = "Number of tries: " + tries;
    }
}
