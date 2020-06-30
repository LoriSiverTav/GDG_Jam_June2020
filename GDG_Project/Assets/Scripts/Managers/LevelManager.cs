using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[Serializable]
public class LevelSol
{
    public int[,] solutionPath;
    public Vector2 startPoint;              // Hold a reference to the starting room of the level
    public Vector2 endPoint;                // Hold a reference to the end room of the level
    public int[] lockSolution;
    public bool isComplete = false;         // TODO Render the treasure piece if is not complete
}

public class LevelManager : MonoBehaviour
{
    public LevelSol[] levels;
    public static LevelManager instance;
    public int lvlIndex;                    // Lets the level manager know what solution path to render
    public int gridSize = 3;                // ie. 3x3 grid

    // Start is called before the first frame update
    void Awake()
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

        int[] tumblerValues = { 0, 1, 2, 3 };
        System.Random rnd = new System.Random();

        foreach (var lvl in levels)
        {
            Vector2 start;
            Vector2 end;

            lvl.solutionPath = GenerateLvlSolution(out start, out end);
            lvl.startPoint = start;
            lvl.endPoint = end;

            var vals = Enumerable.Range(0, 4).OrderBy(r => rnd.Next()).ToArray();
            lvl.lockSolution = vals;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            lvlIndex = 0;
            SceneManager.LoadScene(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            lvlIndex = 1;
            SceneManager.LoadScene(1);
        }
    }

    static public int[,] GenerateLvlSolution(out Vector2 start, out Vector2 end)
    {
        int[,] solutionPath = new int[instance.gridSize, instance.gridSize];

        // Initialize multi-array to have values of -1 - "Empty Room" status
        for (int i = 0; i < instance.gridSize; i++)
            for (int j = 0; j < instance.gridSize; j++)
                solutionPath[i, j] = -1;

        // Counts how many times in a row the solution path goes downwards
        int downCounter = 0;

        // Generate a room for the starting room
        int initialRndCol = Random.Range(0, instance.gridSize);
        solutionPath[0, initialRndCol] = Random.Range(1, 3);

        int lastRow = 0;
        int lastCol = initialRndCol;

        start = new Vector2(lastRow, lastCol);

        for (; ; )
        {
            int rdmDirection = Random.Range(1, 6);

            if (rdmDirection == 1 || rdmDirection == 2)
            {
                // Try to go left
                if (IsPathOk(solutionPath, lastRow, lastCol - 1))
                {
                    solutionPath[lastRow, lastCol - 1] = 1;
                    lastCol--;
                    downCounter = 0;
                }
                else
                {
                    rdmDirection = 5;
                }
            }
            else if (rdmDirection == 3 || rdmDirection == 4)
            {
                // Try to go right
                if (IsPathOk(solutionPath, lastRow, lastCol + 1))
                {
                    solutionPath[lastRow, lastCol + 1] = 1;
                    lastCol++;
                    downCounter = 0;
                }
                else
                {
                    rdmDirection = 5;
                }
            }
            else if (rdmDirection == 5)
            {
                downCounter++;

                // Update the previous room to have an opening going downwards
                int roomToSpawn = downCounter >= 2 ? 4 : 2;
                solutionPath[lastRow, lastCol] = roomToSpawn;

                // Generate a room with an upward opening
                if (IsPathOk(solutionPath, lastRow + 1, lastCol))
                {
                    solutionPath[lastRow + 1, lastCol] = 3;
                    lastRow++;
                }
                else
                {
                    end = new Vector2(lastRow, lastCol);
                    break;
                }
            }
        }

        return solutionPath;
    }

    static public bool IsPathOk(int[,] solutionPath, int row, int col)
    {
        if (row < 0 || row >= instance.gridSize || col < 0 || col >= instance.gridSize) { return false; }

        return solutionPath[row, col] == -1;
    }

    public LevelSol GetLevelData()
    {
        return levels[lvlIndex];
    }
}
