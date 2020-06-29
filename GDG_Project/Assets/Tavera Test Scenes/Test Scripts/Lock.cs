using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tumbler
{
    public GameObject tumbler;
    public bool isPushed = false;
    public int value;
}

public class Lock : MonoBehaviour
{
    public Tumbler[] tumblers;
    public int lockSolutionIndex;
    public int[] solution;
    public int tries = 3;
    public bool isLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        LevelSol levelData = LevelManager.instance.GetLevelData();
        solution = levelData.lockSolution;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTumbler(GameObject tumblerObj)
    {
        Tumbler tumbler = tumblers.FirstOrDefault(t => t.tumbler == tumblerObj);

        if(tumbler != null && !tumbler.isPushed)
        {
            Debug.Log(tumblerObj.name + " has been pressed");
            tumblerObj.transform.localPosition = new Vector3(tumblerObj.transform.localPosition.x, -0.025f, 0);
            tumbler.isPushed = true;
        }
    }
}
