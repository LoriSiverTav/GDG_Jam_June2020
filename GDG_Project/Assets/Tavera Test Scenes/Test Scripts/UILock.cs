using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public bool isPuzzling = false;
    public int[] solution;
    public List<int> playerInput;
    public bool isUnlocked = false;

    private int targetTumbler = 0;
    private Vector3 initialKeyLoc;

    // Start is called before the first frame update
    void Start()
    {
        LevelSol levelData = LevelManager.instance.GetLevelData();
        solution = levelData.lockSolution;

        Debug.Log(tumblers.Length);
        initialKeyLoc = lockPick.gameObject.transform.position;

        if (tumblers.Length == 0) { return; }

        for(int i = 0; i < tumblers.Length; i++)
        {
            tumblers[i].value = i;
            tumblers[i].initialLoc = tumblers[i].tumbler.gameObject.transform.localPosition;
        }

        lockPick.gameObject.transform.position = 
            new Vector3(
                tumblers[targetTumbler].tumbler.gameObject.transform.position.x, 
                lockPick.gameObject.transform.position.y, 
                lockPick.gameObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPuzzling || isUnlocked) { return; }

        if(Input.GetKeyDown(KeyCode.D))
        {
            targetTumbler = targetTumbler + 1 < tumblers.Length ? targetTumbler + 1 : 0;

            lockPick.gameObject.transform.position =
            new Vector3(
                tumblers[targetTumbler].tumbler.gameObject.transform.position.x,
                initialKeyLoc.y,
                lockPick.gameObject.transform.position.z);
        }

        if(Input.GetKeyDown(KeyCode.W))
        {
            LockTumbler tumblerSol = tumblers[targetTumbler];

            if(tumblerSol.isPushed) { return; }

            lockPick.gameObject.transform.position =
            new Vector3(
                lockPick.gameObject.transform.position.x,
                tumblers[targetTumbler].tumbler.gameObject.transform.position.y - 30,
                lockPick.gameObject.transform.position.z);

            tumblerSol.tumbler.transform.localPosition =
            new Vector3(
                tumblerSol.tumbler.gameObject.transform.localPosition.x,
                -25f,
                tumblerSol.tumbler.gameObject.transform.localPosition.z);

            tumblerSol.isPushed = true;
            playerInput.Add(tumblerSol.value);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ResetLock();
        }

        if(playerInput.Count == solution.Length)
        {
            if(IsLockIsComplete())
            {
                isUnlocked = true;
                playerInput.Clear();
            }
        }
    }

    private void ResetLock()
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
                initialKeyLoc.y,
                initialKeyLoc.z);

        playerInput.Clear();
    }

    private bool IsLockIsComplete()
    {
        foreach (var i in playerInput)
        {
            if (playerInput[i] != solution[i])
            {
                ResetLock();
                return false;
            }
        }

        return true;
    }
}
