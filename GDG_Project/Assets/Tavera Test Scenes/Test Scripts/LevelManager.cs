using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelSol
{
    public int[] rooms;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int[][] levelSolutions;

    public LevelSol[] levels;

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
